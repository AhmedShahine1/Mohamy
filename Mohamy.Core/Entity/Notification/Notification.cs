using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mohamy.Core.Entity.Notification
{
    public class Notification : BaseEntity
    {

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public NotificationType NotificationType { get; set; } = NotificationType.None;
        public string? ActionId { get; set; }
    }
}
