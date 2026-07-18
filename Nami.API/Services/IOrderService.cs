using Nami.API.DTOs;

namespace Nami.API.Services
{
    public interface IOrderService
    {
        Task<OrderDto> CreateFromCart(int clientId, CreateOrderRequest request);
        Task<List<OrderDto>> GetByClient(int clientId);
        Task<List<OrderDto>> GetByDelivery(int deliveryId);
        Task<List<OrderDto>> GetAll();
        Task<OrderDto?> GetById(int id);
        Task<OrderDto> UpdateStatus(int orderId, UpdateOrderStatusRequest request);
        Task<OrderDto> AssignDelivery(int orderId, int deliveryId);
        Task<List<OrderDto>> GetAvailable();
        Task<OrderDto> AcceptOrder(int orderId, int deliveryId);
        Task<OrderDto> RejectOrder(int orderId, int deliveryId);
        Task<OrderDto> RateDelivery(int orderId, int clientId, RateDeliveryRequest request);
    }
}
