using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Mappers
{
    public static class CartMappers
    {
        public static CartItemDto ToDto(this CartItem item) => new(
            item.Id,
            item.ProductId,
            item.Product?.Name ?? string.Empty,
            item.Quantity,
            item.UnitPrice,
            item.Subtotal);

        public static CartDto ToDto(this Cart cart) => new(
            cart.Id,
            cart.ClientId,
            cart.Subtotal,
            cart.Items.Select(i => i.ToDto()).ToList());
    }
}
