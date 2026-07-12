using Delivery.API.Models;

namespace Delivery.API.DTOs
{
    public record PaymentDto(
        int Id,
        int OrderId,
        PaymentMethod Method,
        decimal Amount,
        PaymentStatus Status,
        DateTime Date,
        string ResultMessage);
}
