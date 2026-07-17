using Nami.API.Models;

namespace Nami.API.DTOs
{
    public record NotificationDto(
        int Id,
        int UserId,
        string Message,
        NotificationType Type,
        bool IsRead,
        DateTime Date);
}
