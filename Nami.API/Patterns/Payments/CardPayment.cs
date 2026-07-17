namespace Nami.API.Patterns.Payments
{
    public class CardPayment : IPaymentMethod
    {
        public string MethodName => "Tarjeta";

        public PaymentResult ProcessPayment(decimal amount)
            => new(true, $"Pago con tarjeta de {amount:C} procesado correctamente (simulado).");
    }
}
