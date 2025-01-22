using Mohamy.Core.DTO.NotificationViewModel;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId);
        Task ReadNotificationAsync(string notificationId);
        Task CreateNotificationAsync(string userId, string message);
        Task SaveNotificationAsync(SaveNotificationDTO saveNotificationDTO);
    }
}
