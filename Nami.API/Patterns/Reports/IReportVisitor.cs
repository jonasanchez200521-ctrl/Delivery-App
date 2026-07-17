using Nami.API.Models;

namespace Nami.API.Patterns.Reports
{
    // Visitor: separa la lógica de generación de estadísticas de las entidades del dominio.
    public interface IReportVisitor
    {
        void Visit(Order order);
        void Visit(Payment payment);
        void Visit(User user);
    }
}
