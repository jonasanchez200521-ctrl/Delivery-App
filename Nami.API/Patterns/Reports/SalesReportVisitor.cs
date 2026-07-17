using Nami.API.Models;

namespace Nami.API.Patterns.Reports
{
    // Visitor concreto: acumula las estadísticas que consume el dashboard del administrador.
    public class SalesReportVisitor : IReportVisitor
    {
        public int TotalOrders { get; private set; }
        public decimal TotalSales { get; private set; }
        public Dictionary<OrderStatus, int> OrdersByStatus { get; } = new();

        public int TotalPayments { get; private set; }
        public decimal TotalRevenueCollected { get; private set; }

        public Dictionary<string, int> UsersByType { get; } = new();

        public void Visit(Order order)
        {
            TotalOrders++;
            OrdersByStatus[order.Status] = OrdersByStatus.GetValueOrDefault(order.Status) + 1;

            if (order.Status != OrderStatus.Cancelled)
                TotalSales += order.Total;
        }

        public void Visit(Payment payment)
        {
            TotalPayments++;
            if (payment.Status == PaymentStatus.Completed)
                TotalRevenueCollected += payment.Amount;
        }

        public void Visit(User user)
        {
            var type = user switch
            {
                Client => "Client",
                Delivery => "Delivery",
                Administrator => "Administrator",
                _ => "Unknown"
            };

            UsersByType[type] = UsersByType.GetValueOrDefault(type) + 1;
        }
    }
}
