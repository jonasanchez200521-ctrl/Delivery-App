namespace Nami.API.DTOs
{
    public record ProductDto(
        int Id,
        string Name,
        string Description,
        decimal Price,
        int Stock,
        string? ImageUrl,
        int RestaurantId,
        string RestaurantName,
        int CategoryId,
        string CategoryName);

    public record CreateProductRequest(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        int RestaurantId,
        int CategoryId,
        string? ImageUrl = null);

    public record UpdateProductRequest(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        int CategoryId,
        string? ImageUrl = null);
}
