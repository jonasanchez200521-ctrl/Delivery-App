using Nami.API.Data;
using Nami.API.DTOs;
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
                OrderDetails = cart.Items.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Subtotal = i.Subtotal
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _paymentService.ProcessPayment(order.Id, request.PaymentMethod, total);

            _context.CartItems.RemoveRange(cart.Items);
            cart.Items.Clear();
            cart.Subtotal = 0;
            await _context.SaveChangesAsync();

            await _notificationService.SendNotification(
                clientId,
                $"Tu pedido #{order.Id} fue creado y está pendiente de confirmación.",
                NotificationType.Order);

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
    }
}
