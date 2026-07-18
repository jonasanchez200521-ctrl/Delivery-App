namespace Nami.Web.Models
{
    public record OrderDetailDto(
        int Id, int ProductId, string ProductName, int Quantity, decimal UnitPrice, decimal Subtotal, string? Notes);

    public record OrderDto(
        int Id, int ClientId, string ClientName, int? DeliveryId, string? DeliveryName,
        int RestaurantId, string RestaurantName, decimal Total, OrderStatus Status,
        DateTime OrderDate, DateTime? DeliveryDate, string Address,
        double RestaurantLatitude, double RestaurantLongitude,
        double DeliveryLatitude, double DeliveryLongitude,
        int? DeliveryRating, string? DeliveryRatingComment,
        List<OrderDetailDto> Details);

    public record CreateOrderRequest(string Address, PaymentMethod PaymentMethod, string? PromotionCode);

    public record UpdateOrderStatusRequest(OrderStatus Status);

    public record AssignDeliveryRequest(int DeliveryId);

    public record RateDeliveryRequest(int Rating, string? Comment);

    public record PaymentDto(int Id, int OrderId, PaymentMethod Method, decimal Amount, PaymentStatus Status, DateTime Date, string ResultMessage);
}
