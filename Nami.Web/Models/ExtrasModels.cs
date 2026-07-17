namespace Nami.Web.Models
{
    public record PromotionDto(int Id, string Code, decimal Discount, DateTime StartDate, DateTime EndDate, bool IsActive);

    public record CreatePromotionRequest(string Code, decimal Discount, DateTime StartDate, DateTime EndDate);

    public record NotificationDto(int Id, int UserId, string Message, NotificationType Type, bool IsRead, DateTime Date);

    public record ReportDto(
        int TotalOrders,
        decimal TotalSales,
        Dictionary<string, int> OrdersByStatus,
        int TotalPayments,
        decimal TotalRevenueCollected,
        Dictionary<string, int> UsersByType);
}
