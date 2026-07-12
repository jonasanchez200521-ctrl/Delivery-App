using Delivery.API.Models;

namespace Delivery.API.DTOs
{
    public record RestaurantDto(
        int Id,
        string Name,
        string Address,
        string Category,
        RestaurantStatus Status,
        double Rating);

    public record CreateRestaurantRequest(string Name, string Address, string Category);

    public record UpdateRestaurantRequest(
        string Name,
        string Address,
        string Category,
        RestaurantStatus Status,
        double Rating);
}
