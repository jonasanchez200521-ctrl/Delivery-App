using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Patterns.Reports;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class ReportService : IReportService
    {
        private readonly DeliveryDbContext _context;

        public ReportService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task<ReportDto> GenerateReport()
        {
            var visitor = new SalesReportVisitor();

            var orders = await _context.Orders.ToListAsync();
            foreach (var order in orders)
                order.Accept(visitor);

            var payments = await _context.Payments.ToListAsync();
            foreach (var payment in payments)
                payment.Accept(visitor);

            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
                user.Accept(visitor);

            var ordersByStatus = visitor.OrdersByStatus.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value);

            return new ReportDto(
                visitor.TotalOrders,
                visitor.TotalSales,
                ordersByStatus,
                visitor.TotalPayments,
                visitor.TotalRevenueCollected,
                visitor.UsersByType);
        }
    }
}
