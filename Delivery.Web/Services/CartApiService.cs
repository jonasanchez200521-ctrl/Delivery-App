using Delivery.Web.Models;

namespace Delivery.Web.Services
{
    public class CartApiService
    {
        private readonly ApiClient _api;

        public CartApiService(ApiClient api)
        {
            _api = api;
        }

        public Task<CartDto?> GetCart() => _api.GetAsync<CartDto>("api/cart");

        public Task<CartDto?> AddItem(AddCartItemRequest request) =>
            _api.PostAsync<CartDto>("api/cart/items", request);

        public Task<CartDto?> UpdateItem(int itemId, UpdateCartItemRequest request) =>
            _api.PutAsync<CartDto>($"api/cart/items/{itemId}", request);

        public Task<CartDto?> RemoveItem(int itemId) => _api.DeleteAsync<CartDto>($"api/cart/items/{itemId}");

        public Task ClearCart() => _api.DeleteAsync("api/cart");
    }
}
