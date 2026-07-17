namespace Nami.Web.Models
{
    public record RestaurantDto(int Id, string Name, string Address, string Category, RestaurantStatus Status, double Rating);

    public record CreateRestaurantRequest(string Name, string Address, string Category);

    public record UpdateRestaurantRequest(string Name, string Address, string Category, RestaurantStatus Status, double Rating);

    public record CategoryDto(int Id, string Name, string Description);

    public record CreateCategoryRequest(string Name, string Description);

    public record ProductDto(
        int Id, string Name, string Description, decimal Price, int Stock,
        int RestaurantId, string RestaurantName, int CategoryId, string CategoryName);

    public record CreateProductRequest(
        string Name, string Description, decimal Price, int Stock, int RestaurantId, int CategoryId);

    public record UpdateProductRequest(string Name, string Description, decimal Price, int Stock, int CategoryId);
}
