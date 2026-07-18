using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Mappers
{
    public static class OrderMappers
    {
        public static OrderDetailDto ToDto(this OrderDetail detail) => new(
            detail.Id,
            detail.ProductId,
            detail.Product?.Name ?? string.Empty,
            detail.Quantity,
            detail.UnitPrice,
            detail.Subtotal,
            detail.Notes);

        public static OrderDto ToDto(this Order order) => new(
            order.Id,
            order.ClientId,
            order.Client?.FullName ?? string.Empty,
            order.DeliveryId,
            order.Delivery?.FullName,
            order.RestaurantId,
            order.Restaurant?.Name ?? string.Empty,
            order.Total,
            order.Status,
            order.OrderDate,
            order.DeliveryDate,
            order.Address,
            order.Restaurant?.Latitude ?? 0,
            order.Restaurant?.Longitude ?? 0,
            order.DeliveryLatitude,
            order.DeliveryLongitude,
            order.DeliveryRating,
            order.DeliveryRatingComment,
            order.OrderDetails.Select(d => d.ToDto()).ToList());
    }
}
