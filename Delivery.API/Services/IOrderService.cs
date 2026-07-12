using Delivery.API.DTOs;

namespace Delivery.API.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateFromCart(int clientId, CreateOrderRequest request);
        Task<List<OrderDto>> GetByClient(int clientId);
        Task<List<OrderDto>> GetByCourier(int courierId);
        Task<List<OrderDto>> GetAll();
        Task<OrderDto?> GetById(int id);
        Task<OrderDto> UpdateStatus(int orderId, UpdateOrderStatusRequest request);
        Task<OrderDto> AssignCourier(int orderId, int courierId);
    }
}
