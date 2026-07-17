using Nami.API.Data;
using Nami.API.DTOs;
using Nami.API.Mappers;
using Nami.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Nami.API.Services
{
    public class NotificationService : INotification
    {
        private readonly DeliveryDbContext _context;

        public NotificationService(DeliveryDbContext context)
        {
            _context = context;
        }

        public async Task SendNotification(int userId, string message, NotificationType type)
        {
            _context.Notifications.Add(new Notification
            {
                UserId = userId,
                Message = message,
                Type = type,
                IsRead = false,
                Date = DateTime.UtcNow
            });
            await _context.SaveChangesAsync();
        }

        public async Task<List<NotificationDto>> GetUserNotifications(int userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.Date)
                .Select(n => n.ToDto())
                .ToListAsync();
        }

        public async Task MarkAsRead(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId)
                ?? throw new InvalidOperationException("Notificación no encontrada.");
            notification.IsRead = true;
            await _context.SaveChangesAsync();
        }
    }
}
