using Nami.API.DTOs;
using Nami.API.Models;

namespace Nami.API.Services
{
    // Interfaz de dominio (UML): contrato de notificaciones.
    public interface INotification
    {
        Task SendNotification(int userId, string message, NotificationType type);
        Task<List<NotificationDto>> GetUserNotifications(int userId);
        Task MarkAsRead(int notificationId);
    }
}
