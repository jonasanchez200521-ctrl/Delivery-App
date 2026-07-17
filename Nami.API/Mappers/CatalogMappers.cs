using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Mappers
{
    public static class CatalogMappers
    {
        public static RestaurantDto ToDto(this Restaurant restaurant) => new(
            restaurant.Id,
            restaurant.Name,
            restaurant.Address,
            restaurant.Category,
            restaurant.Status,
            restaurant.Rating);

        public static CategoryDto ToDto(this Category category) => new(
            category.Id,
            category.Name,
            category.Description);

        public static ProductDto ToDto(this Product product) => new(
            product.Id,
            product.Name,
            product.Description,
            product.Price,
            product.Stock,
            product.RestaurantId,
            product.Restaurant?.Name ?? string.Empty,
            product.CategoryId,
            product.Category?.Name ?? string.Empty);
    }
}
