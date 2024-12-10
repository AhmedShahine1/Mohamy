using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.Core.Entity.ChatData
{
    public class Chat : BaseEntity
    {
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
    }
}
