namespace Delivery.API.DTOs
{
    public record CartItemDto(
        int Id,
        int ProductId,
        string ProductName,
        int Quantity,
        decimal UnitPrice,
        decimal Subtotal);

    public record CartDto(int Id, int ClientId, decimal Subtotal, List<CartItemDto> Items);

    public record AddCartItemRequest(int ProductId, int Quantity);

    public record UpdateCartItemRequest(int Quantity);
}
