using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> ProcessPayment(int orderId, PaymentMethod method, decimal amount);
        Task<PaymentDto?> GetByOrder(int orderId);
    }
}
