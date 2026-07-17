using Nami.API.Models;

namespace Nami.API.Patterns.Payments
{
    // Abstract Factory: crea la implementación correcta de IPaymentMethod
    // según el método de pago elegido, sin que el resto del sistema conozca las clases concretas.
    public interface IPaymentFactory
    {
        IPaymentMethod CreatePaymentMethod(PaymentMethod method);
    }
}
