namespace Nami.API.Patterns.Payments
{
    public class CashPayment : IPaymentMethod
    {
        public string MethodName => "Efectivo";

        public PaymentResult ProcessPayment(decimal amount)
            => new(true, $"Pago en efectivo de {amount:C} registrado. Se cobrará contra entrega.");
    }
}
