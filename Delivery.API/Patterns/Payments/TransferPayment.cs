namespace Delivery.API.Patterns.Payments
{
    public class TransferPayment : IPaymentMethod
    {
        public string MethodName => "Transferencia";

        public PaymentResult ProcessPayment(decimal amount)
            => new(true, $"Transferencia de {amount:C} registrada (simulada).");
    }
}
