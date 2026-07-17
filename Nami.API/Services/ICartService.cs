using Nami.API.DTOs;

namespace Nami.API.Services
{
    public interface ICartService
    {
        Task<CartDto> GetCart(int clientId);
        Task<CartDto> AddItem(int clientId, AddCartItemRequest request);
        Task<CartDto> UpdateItem(int clientId, int itemId, UpdateCartItemRequest request);
        Task<CartDto> RemoveItem(int clientId, int itemId);
        Task ClearCart(int clientId);
    }
}
