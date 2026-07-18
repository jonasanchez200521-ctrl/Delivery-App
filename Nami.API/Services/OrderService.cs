using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Exceptions;
using Nami.API.Mappers;
using Nami.API.Models;
using Nami.API.Patterns;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly DeliveryDbContext _context;
        private readonly ICalculate _calculate;
        private readonly IPaymentService _paymentService;
        private readonly IPromotionService _promotionService;
        private readonly INotification _notificationService;

        public OrderService(
            DeliveryDbContext context,
            ICalculate calculate,
            IPaymentService paymentService,
            IPromotionService promotionService,
            INotification notificationService)
        {
            _context = context;
            _calculate = calculate;
            _paymentService = paymentService;
            _promotionService = promotionService;
            _notificationService = notificationService;
        }

        // Sin geocodificación real disponible: el punto de entrega se fija en una
        // coordenada representativa de Quito (mismo criterio que el prototipo de diseño).
        private const double DefaultDeliveryLatitude = -0.1976;
        private const double DefaultDeliveryLongitude = -78.4903;

        private static IQueryable<Order> IncludeGraph(IQueryable<Order> query) => query
            .Include(o => o.Client)
            .Include(o => o.Delivery)
            .Include(o => o.Restaurant)
            .Include(o => o.OrderDetails).ThenInclude(d => d.Product);

        public async Task<OrderDto> CreateFromCart(int clientId, CreateOrderRequest request)
        {
            var cart = await _context.Carts
                .Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.ClientId == clientId)
                ?? throw new InvalidOperationException("El carrito está vacío.");

            if (cart.Items.Count == 0)
                throw new InvalidOperationException("El carrito está vacío.");

            var restaurantId = cart.Items.First().Product.RestaurantId;
            var subtotal = cart.Items.Sum(i => i.Subtotal);

            var discount = 0m;
            if (!string.IsNullOrWhiteSpace(request.PromotionCode))
            {
                var promotion = await _promotionService.GetActiveByCode(request.PromotionCode);
                if (promotion is not null)
                    discount = subtotal * promotion.Discount / 100m;
            }

            var taxable = subtotal - discount;
            var iva = _calculate.CalculateIVA(taxable);
            var total = taxable + iva;

            var order = new Order
            {
                ClientId = clientId,
                RestaurantId = restaurantId,
                Total = total,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                Address = request.Address,
                DeliveryLatitude = DefaultDeliveryLatitude,
                DeliveryLongitude = DefaultDeliveryLongitude,
                OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal,
                    Notes = i.Notes
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            var payment = await _paymentService.ProcessPayment(order.Id, request.PaymentMethod, total);

            // Un pago aprobado confirma el pedido y lo deja listo para que un repartidor
            // lo tome (ver OrderService.GetAvailable): sin esto, ningún pedido nuevo
            // aparecería jamás como disponible.
            order.Status = payment.Status == PaymentStatus.Completed ? OrderStatus.Confirmed : OrderStatus.Pending;

            _context.CartItems.RemoveRange(cart.Items);
            cart.Items.Clear();
            cart.Subtotal = 0;
            await _context.SaveChangesAsync();

            var confirmationMessage = order.Status == OrderStatus.Confirmed
                ? $"Tu pedido #{order.Id} fue confirmado y está esperando repartidor."
                : $"Tu pedido #{order.Id} fue creado y está pendiente de confirmación.";

            await _notificationService.SendNotification(clientId, confirmationMessage, NotificationType.Order);

            return (await GetById(order.Id))!;
        }

        public async Task<List<OrderDto>> GetByClient(int clientId) =>
            await IncludeGraph(_context.Orders)
                .Where(o => o.ClientId == clientId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => o.ToDto())
                .ToListAsync();

        public async Task<List<OrderDto>> GetByDelivery(int deliveryId) =>
            await IncludeGraph(_context.Orders)
                .Where(o => o.DeliveryId == deliveryId)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => o.ToDto())
                .ToListAsync();

        public async Task<List<OrderDto>> GetAll() =>
            await IncludeGraph(_context.Orders)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => o.ToDto())
                .ToListAsync();

        public async Task<OrderDto?> GetById(int id)
        {
            var order = await IncludeGraph(_context.Orders).FirstOrDefaultAsync(o => o.Id == id);
            return order?.ToDto();
        }

        public async Task<OrderDto> UpdateStatus(int orderId, UpdateOrderStatusRequest request)
        {
            var order = await IncludeGraph(_context.Orders).FirstOrDefaultAsync(o => o.Id == orderId)
                ?? throw new InvalidOperationException("Pedido no encontrado.");

            order.Status = request.Status;
            if (request.Status == OrderStatus.Delivered)
                order.DeliveryDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            await _notificationService.SendNotification(
                order.ClientId,
                $"Tu pedido #{order.Id} cambió de estado a {order.Status}.",
                NotificationType.Order);

            return order.ToDto();
        }

        public async Task<OrderDto> AssignDelivery(int orderId, int deliveryId)
        {
            var order = await IncludeGraph(_context.Orders).FirstOrDefaultAsync(o => o.Id == orderId)
                ?? throw new InvalidOperationException("Pedido no encontrado.");

            var delivery = await _context.Deliveries.FindAsync(deliveryId)
                ?? throw new InvalidOperationException("Repartidor no encontrado.");

            if (!delivery.Availability)
                throw new InvalidOperationException("El repartidor no está disponible.");

            order.DeliveryId = deliveryId;
            if (order.Status == OrderStatus.Pending)
                order.Status = OrderStatus.Confirmed;

            await _context.SaveChangesAsync();

            await _notificationService.SendNotification(
                deliveryId,
                $"Se te asignó el pedido #{order.Id}.",
                NotificationType.Order);

            return order.ToDto();
        }

        public async Task<List<OrderDto>> GetAvailable() =>
            await IncludeGraph(_context.Orders)
                .Where(o => o.Status == OrderStatus.Confirmed && o.DeliveryId == null)
                .OrderBy(o => o.OrderDate)
                .Select(o => o.ToDto())
                .ToListAsync();

        public async Task<OrderDto> AcceptOrder(int orderId, int deliveryId)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId)
                ?? throw new InvalidOperationException("Repartidor no encontrado.");

            if (!delivery.Availability)
                throw new InvalidOperationException("No estás disponible para tomar pedidos.");

            // Un repartidor no puede tener más de una entrega activa a la vez.
            var hasActiveOrder = await _context.Orders.AnyAsync(o =>
                o.DeliveryId == deliveryId && o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled);
            if (hasActiveOrder)
                throw new OrderConflictException("Ya tienes una entrega activa. Termina tu entrega actual para tomar otra.");

            // UPDATE atómico condicionado al estado esperado: evita que dos repartidores
            // tomen el mismo pedido a la vez (la segunda ejecución afecta 0 filas).
            var rows = await _context.Orders
                .Where(o => o.Id == orderId && o.Status == OrderStatus.Confirmed && o.DeliveryId == null)
                .ExecuteUpdateAsync(s => s.SetProperty(o => o.DeliveryId, deliveryId));

            if (rows == 0)
            {
                var exists = await _context.Orders.AnyAsync(o => o.Id == orderId);
                if (!exists)
                    throw new InvalidOperationException("Pedido no encontrado.");

                throw new OrderConflictException("Este pedido ya fue tomado por otro repartidor.");
            }

            var order = await IncludeGraph(_context.Orders).FirstAsync(o => o.Id == orderId);

            await _notificationService.SendNotification(
                order.ClientId,
                $"Tu pedido #{order.Id} fue tomado por {delivery.FullName}.",
                NotificationType.Order);

            return order.ToDto();
        }

        public async Task<OrderDto> RejectOrder(int orderId, int deliveryId)
        {
            var order = await IncludeGraph(_context.Orders).FirstOrDefaultAsync(o => o.Id == orderId)
                ?? throw new InvalidOperationException("Pedido no encontrado.");

            if (order.DeliveryId != deliveryId)
                throw new InvalidOperationException("No tienes asignado este pedido.");

            if (order.Status is OrderStatus.PickedUp or OrderStatus.Delivered or OrderStatus.Cancelled)
                throw new InvalidOperationException("No se puede rechazar una entrega que ya fue recogida o finalizada.");

            order.DeliveryId = null;
            order.Status = OrderStatus.Confirmed;
            await _context.SaveChangesAsync();

            await _notificationService.SendNotification(
                order.ClientId,
                $"El repartidor liberó tu pedido #{order.Id}; buscaremos uno nuevo.",
                NotificationType.Order);

            return order.ToDto();
        }

        public async Task<OrderDto> RateDelivery(int orderId, int clientId, RateDeliveryRequest request)
        {
            if (request.Rating is < 1 or > 5)
                throw new InvalidOperationException("La calificación debe estar entre 1 y 5.");

            var order = await IncludeGraph(_context.Orders).FirstOrDefaultAsync(o => o.Id == orderId)
                ?? throw new InvalidOperationException("Pedido no encontrado.");

            if (order.ClientId != clientId)
                throw new InvalidOperationException("No puedes calificar un pedido que no es tuyo.");

            if (order.Status != OrderStatus.Delivered)
                throw new InvalidOperationException("Solo puedes calificar pedidos ya entregados.");

            if (order.DeliveryId is null)
                throw new InvalidOperationException("Este pedido no tiene repartidor asignado.");

            if (order.DeliveryRating is not null)
                throw new InvalidOperationException("Ya calificaste este pedido.");

            order.DeliveryRating = request.Rating;
            order.DeliveryRatingComment = request.Comment;
            await _context.SaveChangesAsync();

            var deliveryId = order.DeliveryId.Value;
            var average = await _context.Orders
                .Where(o => o.DeliveryId == deliveryId && o.DeliveryRating != null)
                .AverageAsync(o => (double)o.DeliveryRating!.Value);

            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery is not null)
            {
                delivery.Rating = Math.Round(average, 2);
                await _context.SaveChangesAsync();
            }

            await _notificationService.SendNotification(
                deliveryId,
                $"Recibiste una calificación de {request.Rating}★ en el pedido #{order.Id}.",
                NotificationType.Order);

            return order.ToDto();
        }
    }
}
