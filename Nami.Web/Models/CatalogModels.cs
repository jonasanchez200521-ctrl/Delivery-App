namespace Nami.Web.Models
{
    public record RestaurantDto(
        int Id, string Name, string Address, string Category, RestaurantStatus Status, double Rating,
        string? ImageUrl, double Latitude, double Longitude);

    public record CreateRestaurantRequest(
        string Name, string Address, string Category,
        string? ImageUrl = null, double Latitude = 0, double Longitude = 0);

    public record UpdateRestaurantRequest(
        string Name, string Address, string Category, RestaurantStatus Status, double Rating,
        string? ImageUrl = null, double Latitude = 0, double Longitude = 0);

    public record CategoryDto(int Id, string Name, string Description);

    public record CreateCategoryRequest(string Name, string Description);

    public record ProductDto(
        int Id, string Name, string Description, decimal Price, int Stock, string? ImageUrl,
        int RestaurantId, string RestaurantName, int CategoryId, string CategoryName);

    public record CreateProductRequest(
        string Name, string Description, decimal Price, int Stock, int RestaurantId, int CategoryId,
        string? ImageUrl = null);

    public record UpdateProductRequest(
        string Name, string Description, decimal Price, int Stock, int CategoryId, string? ImageUrl = null);
}
