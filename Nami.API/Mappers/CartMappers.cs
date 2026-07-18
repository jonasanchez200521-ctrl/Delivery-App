using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Mappers
{
    public static class CartMappers
    {
        public static CartItemDto ToDto(this CartItem item) => new(
            item.Id,
            item.ProductId,
            item.Product?.Name ?? string.Empty,
            item.Product?.ImageUrl,
            item.Quantity,
            item.UnitPrice,
            item.Subtotal,
            item.Notes);

        public static CartDto ToDto(this Cart cart) => new(
            cart.Id,
            cart.ClientId,
            cart.Subtotal,
            cart.Items.Select(i => i.ToDto()).ToList());
    }
}
