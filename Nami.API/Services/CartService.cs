using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class CartService : ICartService
    {
        private readonly DeliveryDbContext _context;

        public CartService(DeliveryDbContext context)
        {
            _context = context;
        }

        private async Task<Cart> GetOrCreateCartEntity(int clientId)
        {
            var cart = await _context.Carts
                .Include(c => c.Items).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.ClientId == clientId);

            if (cart is null)
            {
                cart = new Cart { ClientId = clientId, Subtotal = 0 };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        private static void RecalculateSubtotal(Cart cart) =>
            cart.Subtotal = cart.Items.Sum(i => i.Subtotal);

        public async Task<CartDto> GetCart(int clientId)
        {
            var cart = await GetOrCreateCartEntity(clientId);
            return cart.ToDto();
        }

        public async Task<CartDto> AddItem(int clientId, AddCartItemRequest request)
        {
            if (request.Quantity <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            var product = await _context.Products.FindAsync(request.ProductId)
                ?? throw new InvalidOperationException("Producto no encontrado.");

            var cart = await GetOrCreateCartEntity(clientId);

            // El carrito solo admite productos de un mismo restaurante a la vez.
            var hasOtherRestaurant = cart.Items.Any(i => i.Product.RestaurantId != product.RestaurantId);
            if (hasOtherRestaurant)
            {
                _context.CartItems.RemoveRange(cart.Items);
                cart.Items.Clear();
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem is not null)
            {
                existingItem.Quantity += request.Quantity;
                if (request.Notes is not null)
                    existingItem.Notes = request.Notes;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    UnitPrice = product.Price,
                    Notes = request.Notes
                };
                cart.Items.Add(newItem);
                _context.CartItems.Add(newItem);
            }

            RecalculateSubtotal(cart);
            await _context.SaveChangesAsync();

            return (await GetCart(clientId));
        }

        public async Task<CartDto> UpdateItem(int clientId, int itemId, UpdateCartItemRequest request)
        {
            if (request.Quantity <= 0)
                throw new InvalidOperationException("La cantidad debe ser mayor a cero.");

            var cart = await GetOrCreateCartEntity(clientId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId)
                ?? throw new InvalidOperationException("El producto no está en el carrito.");

            item.Quantity = request.Quantity;
            RecalculateSubtotal(cart);
            await _context.SaveChangesAsync();

            return cart.ToDto();
        }

        public async Task<CartDto> RemoveItem(int clientId, int itemId)
        {
            var cart = await GetOrCreateCartEntity(clientId);
            var item = cart.Items.FirstOrDefault(i => i.Id == itemId)
                ?? throw new InvalidOperationException("El producto no está en el carrito.");

            cart.Items.Remove(item);
            _context.CartItems.Remove(item);
            RecalculateSubtotal(cart);
            await _context.SaveChangesAsync();

            return cart.ToDto();
        }

        public async Task ClearCart(int clientId)
        {
            var cart = await GetOrCreateCartEntity(clientId);
            _context.CartItems.RemoveRange(cart.Items);
            cart.Items.Clear();
            cart.Subtotal = 0;
            await _context.SaveChangesAsync();
        }
    }
}
