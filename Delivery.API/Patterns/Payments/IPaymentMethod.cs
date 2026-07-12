namespace Delivery.API.Patterns.Payments
{
    public record PaymentResult(bool Success, string Message);

    // Producto abstracto del patrón Abstract Factory.
    public interface IPaymentMethod
    {
        string MethodName { get; }

        // Simulado: no hay pasarela real, siempre confirma el pago.
        PaymentResult ProcessPayment(decimal amount);
    }
}
