using Delivery.API.DTOs;
using Delivery.API.Models;

namespace Delivery.API.Services
{
    // Interfaz de dominio (UML): contrato de notificaciones.
    public interface INotification
    {
        Task SendNotification(int userId, string message, NotificationType type);
        Task<List<NotificationDto>> GetUserNotifications(int userId);
        Task MarkAsRead(int notificationId);
    }
}
