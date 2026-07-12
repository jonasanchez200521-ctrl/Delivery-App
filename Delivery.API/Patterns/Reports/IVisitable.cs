namespace Delivery.API.Patterns.Reports
{
    // Elemento visitable del patrón Visitor: Order, Payment y User lo implementan.
    public interface IVisitable
    {
        void Accept(IReportVisitor visitor);
    }
}
