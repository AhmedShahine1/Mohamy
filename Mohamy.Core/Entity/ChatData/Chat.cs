using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.Files;

namespace Mohamy.Core.Entity.ChatData
{
    public class Chat : BaseEntity
    {
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string Message { get; set; }
        public string ImagesId { get; set; }
        public Images Images { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
