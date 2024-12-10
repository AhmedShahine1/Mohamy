using Microsoft.AspNetCore.SignalR;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ChatViewModel;

namespace Mohamy.BusinessLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly INotificationService _notificationService;

        public ChatHub(IChatService chatService, INotificationService notificationService)
        {
            _chatService = chatService;
            _notificationService = notificationService;
        }

        public async Task SendMessage(ChatDTO message)
        {
            var savedMessage = await _chatService.SendMessageAsync(message);
            await _notificationService.CreateNotificationAsync(message.ReceiverId, "You have a new message");

            // Notify receiver
            await Clients.User(message.ReceiverId).SendAsync("ReceiveMessage", savedMessage);

            // Notify sender
            await Clients.Caller.SendAsync("MessageSent", savedMessage);
        }

        public async Task GetMessages(string senderId, string receiverId)
        {
            var messages = await _chatService.GetChatsAsync(senderId, receiverId);
            await Clients.Caller.SendAsync("ReceiveMessages", messages);
        }
    }
}
