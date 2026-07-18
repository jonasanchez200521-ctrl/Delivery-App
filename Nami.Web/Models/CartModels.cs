namespace Nami.Web.Models
{
    public record CartItemDto(
        int Id, int ProductId, string ProductName, string? ProductImageUrl,
        int Quantity, decimal UnitPrice, decimal Subtotal, string? Notes);

    public record CartDto(int Id, int ClientId, decimal Subtotal, List<CartItemDto> Items);

    public record AddCartItemRequest(int ProductId, int Quantity, string? Notes = null);

    public record UpdateCartItemRequest(int Quantity);
}
