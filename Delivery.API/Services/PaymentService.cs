using Delivery.API.Data;
using Delivery.API.DTOs;
using Delivery.API.Mappers;
using Delivery.API.Models;
using Delivery.API.Patterns.Payments;
using Microsoft.EntityFrameworkCore;

namespace Delivery.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly DeliveryDbContext _context;
        private readonly IPaymentFactory _paymentFactory;

        public PaymentService(DeliveryDbContext context, IPaymentFactory paymentFactory)
        {
            _context = context;
            _paymentFactory = paymentFactory;
        }

        public async Task<PaymentDto> ProcessPayment(int orderId, PaymentMethod method, decimal amount)
        {
            // Abstract Factory: obtiene la estrategia de pago concreta sin conocer su clase.
            var paymentMethod = _paymentFactory.CreatePaymentMethod(method);
            var result = paymentMethod.ProcessPayment(amount);

            var payment = new Payment
            {
                OrderId = orderId,
                Method = method,
                Amount = amount,
                Status = result.Success ? PaymentStatus.Completed : PaymentStatus.Failed,
                Date = DateTime.UtcNow
            };

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return payment.ToDto(result.Message);
        }

        public async Task<PaymentDto?> GetByOrder(int orderId)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
            return payment?.ToDto();
        }
    }
}
