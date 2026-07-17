using Nami.API.Patterns.Reports;

namespace Nami.API.Models
{
    // Pago SIMULADO: se registra y se marca Completed/Failed sin pasarela real.
    public class Payment : IVisitable
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public PaymentMethod Method { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public DateTime Date { get; set; }

        public void Accept(IReportVisitor visitor) => visitor.Visit(this);
    }
}
