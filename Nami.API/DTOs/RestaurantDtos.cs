using Nami.API.Models;

namespace Nami.API.DTOs
{
    public record RestaurantDto(
        int Id,
        string Name,
        string Address,
        string Category,
        RestaurantStatus Status,
        double Rating,
        string? ImageUrl,
        double Latitude,
        double Longitude);

    public record CreateRestaurantRequest(
        string Name,
        string Address,
        string Category,
        string? ImageUrl = null,
        double Latitude = 0,
        double Longitude = 0);

    public record UpdateRestaurantRequest(
        string Name,
        string Address,
        string Category,
        RestaurantStatus Status,
        double Rating,
        string? ImageUrl = null,
        double Latitude = 0,
        double Longitude = 0);
}
