using Mohamy.Core.DTO.NotificationViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDTO>> GetNotificationsAsync(string userId);
        Task CreateNotificationAsync(string userId, string message);
    }
}
