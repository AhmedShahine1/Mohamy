using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.NotificationViewModel;
using Mohamy.Core.Entity.Notification;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId)
        {
            var notifications = await _unitOfWork.NotificationRepository.FindAllAsync(n => n.UserId == userId);
            return notifications.Select(n => new NotificationDTO
            {
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            });
        }

        public async Task CreateNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message
            };

            await _unitOfWork.NotificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
