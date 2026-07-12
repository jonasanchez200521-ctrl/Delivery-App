namespace Delivery.API.DTOs
{
    public record ReportDto(
        int TotalOrders,
        decimal TotalSales,
        Dictionary<string, int> OrdersByStatus,
        int TotalPayments,
        decimal TotalRevenueCollected,
        Dictionary<string, int> UsersByType);
}
