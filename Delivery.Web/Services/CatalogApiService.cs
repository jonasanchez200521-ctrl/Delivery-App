using Delivery.Web.Models;

namespace Delivery.Web.Services
{
    public class CatalogApiService
    {
        private readonly ApiClient _api;

        public CatalogApiService(ApiClient api)
        {
            _api = api;
        }

        // Restaurantes
        public async Task<List<RestaurantDto>> GetRestaurants() =>
            await _api.GetAsync<List<RestaurantDto>>("api/restaurants") ?? [];

        public Task<RestaurantDto?> GetRestaurant(int id) => _api.GetAsync<RestaurantDto>($"api/restaurants/{id}");

        public Task<RestaurantDto?> CreateRestaurant(CreateRestaurantRequest request) =>
            _api.PostAsync<RestaurantDto>("api/restaurants", request);

        public Task<RestaurantDto?> UpdateRestaurant(int id, UpdateRestaurantRequest request) =>
            _api.PutAsync<RestaurantDto>($"api/restaurants/{id}", request);

        public Task DeleteRestaurant(int id) => _api.DeleteAsync($"api/restaurants/{id}");

        // Categorías
        public async Task<List<CategoryDto>> GetCategories() =>
            await _api.GetAsync<List<CategoryDto>>("api/categories") ?? [];

        public Task<CategoryDto?> CreateCategory(CreateCategoryRequest request) =>
            _api.PostAsync<CategoryDto>("api/categories", request);

        public Task DeleteCategory(int id) => _api.DeleteAsync($"api/categories/{id}");

        // Productos
        public async Task<List<ProductDto>> GetProducts(int? restaurantId = null)
        {
            var url = restaurantId.HasValue ? $"api/products?restaurantId={restaurantId}" : "api/products";
            return await _api.GetAsync<List<ProductDto>>(url) ?? [];
        }

        public Task<ProductDto?> CreateProduct(CreateProductRequest request) =>
            _api.PostAsync<ProductDto>("api/products", request);

        public Task<ProductDto?> UpdateProduct(int id, UpdateProductRequest request) =>
            _api.PutAsync<ProductDto>($"api/products/{id}", request);

        public Task DeleteProduct(int id) => _api.DeleteAsync($"api/products/{id}");
    }
}
