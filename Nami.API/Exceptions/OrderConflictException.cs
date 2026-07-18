namespace Nami.API.Exceptions
{
    // Se lanza cuando dos repartidores intentan tomar el mismo pedido a la vez.
    public class OrderConflictException : Exception
    {
        public OrderConflictException(string message) : base(message)
        {
        }
    }
}
