using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Mappers
{
    public static class OrderMappers
    {
        public static OrderDetailDto ToDto(this OrderDetail detail) => new(
            detail.Id,
            detail.ProductId,
            detail.Product?.Name ?? string.Empty,
            detail.Quantity,
            detail.UnitPrice,
            detail.Subtotal);

        public static OrderDto ToDto(this Order order) => new(
            order.Id,
            order.ClientId,
            order.Client?.FullName ?? string.Empty,
            order.CourierId,
            order.Courier?.FullName,
            order.RestaurantId,
            order.Restaurant?.Name ?? string.Empty,
            order.Total,
            order.Status,
            order.OrderDate,
            order.DeliveryDate,
            order.Address,
            order.OrderDetails.Select(d => d.ToDto()).ToList());
    }
}
