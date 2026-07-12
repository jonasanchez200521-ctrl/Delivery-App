using Delivery.API.Models;

namespace Delivery.API.Patterns.Payments
{
    public class PaymentFactory : IPaymentFactory
    {
        public IPaymentMethod CreatePaymentMethod(PaymentMethod method) => method switch
        {
            PaymentMethod.Cash => new CashPayment(),
            PaymentMethod.Card => new CardPayment(),
            PaymentMethod.Transfer => new TransferPayment(),
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, "Método de pago no soportado")
        };
    }
}
