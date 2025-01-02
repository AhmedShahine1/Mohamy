using Mohamy.Core.DTO.NotificationViewModel;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId);
        Task CreateNotificationAsync(string userId, string message);
    }
}
