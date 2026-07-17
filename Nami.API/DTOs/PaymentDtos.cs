using Nami.API.Models;

namespace Nami.API.DTOs
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
