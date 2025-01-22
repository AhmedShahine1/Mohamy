using Mohamy.Core.Helpers;

namespace Mohamy.Core.DTO.NotificationViewModel
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }
        public string? ActionId { get; set; }
    }

        public class SaveNotificationDTO
        {
            public string UserId { get; set; }
            public string Title { get; set; }
            public string Message { get; set; }
            public string DeviceId { get; set; }
            public NotificationType NotificationType { get; set; }
            public string? ActionId { get; set; }
        }
}
