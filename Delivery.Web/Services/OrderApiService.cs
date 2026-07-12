using Delivery.Web.Models;

namespace Delivery.Web.Services
{
    public class OrderApiService
    {
        private readonly ApiClient _api;

        public OrderApiService(ApiClient api)
        {
            _api = api;
        }

        public Task<OrderDto?> Create(CreateOrderRequest request) => _api.PostAsync<OrderDto>("api/orders", request);

        public async Task<List<OrderDto>> GetMine() => await _api.GetAsync<List<OrderDto>>("api/orders/mine") ?? [];

        public async Task<List<OrderDto>> GetMyDeliveries() =>
            await _api.GetAsync<List<OrderDto>>("api/orders/courier/mine") ?? [];

        public async Task<List<OrderDto>> GetAll() => await _api.GetAsync<List<OrderDto>>("api/orders") ?? [];

        public Task<OrderDto?> GetById(int id) => _api.GetAsync<OrderDto>($"api/orders/{id}");

        public Task<OrderDto?> UpdateStatus(int id, UpdateOrderStatusRequest request) =>
            _api.PatchAsync<OrderDto>($"api/orders/{id}/status", request);

        public Task<OrderDto?> AssignCourier(int id, AssignCourierRequest request) =>
            _api.PatchAsync<OrderDto>($"api/orders/{id}/assign-courier", request);

        public Task<PaymentDto?> GetPayment(int orderId) => _api.GetAsync<PaymentDto>($"api/payments/order/{orderId}");
    }
}
