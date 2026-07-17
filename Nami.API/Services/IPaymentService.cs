using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Services
{
    public interface IPaymentService
    {
        Task<PaymentDto> ProcessPayment(int orderId, PaymentMethod method, decimal amount);
        Task<PaymentDto?> GetByOrder(int orderId);
    }
}
