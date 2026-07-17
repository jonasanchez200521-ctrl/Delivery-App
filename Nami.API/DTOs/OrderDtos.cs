using Nami.API.Models;

namespace Nami.API.DTOs
{
    public record OrderDetailDto(
        int Id,
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal);

    public record OrderDto(
        int Id,
        int ClientId,
        string ClientName,
        int? DeliveryId,
        string? DeliveryName,
        int RestaurantId,
        string RestaurantName,
        decimal Total,
        OrderStatus Status,
        DateTime OrderDate,
        DateTime? DeliveryDate,
        string Address,
        List<OrderDetailDto> Details);

    // Crea la orden a partir del carrito actual del cliente autenticado.
    public record CreateOrderRequest(string Address, PaymentMethod PaymentMethod, string? PromotionCode);

    public record UpdateOrderStatusRequest(OrderStatus Status);

    public record AssignDeliveryRequest(int DeliveryId);
}
