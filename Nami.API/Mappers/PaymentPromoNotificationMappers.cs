using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Mappers
{
    public static class PaymentPromoNotificationMappers
    {
        public static PaymentDto ToDto(this Payment payment, string resultMessage = "") => new(
            payment.Id,
            payment.OrderId,
            payment.Method,
            payment.Amount,
            payment.Status,
            payment.Date,
            resultMessage);

        public static PromotionDto ToDto(this Promotion promotion) => new(
            promotion.Id,
            promotion.Code,
            promotion.Discount,
            promotion.StartDate,
            promotion.EndDate,
            promotion.IsActive);

        public static NotificationDto ToDto(this Notification notification) => new(
            notification.Id,
            notification.UserId,
            notification.Message,
            notification.Type,
            notification.IsRead,
            notification.Date);
    }
}
