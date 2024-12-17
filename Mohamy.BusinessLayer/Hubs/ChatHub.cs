using Microsoft.AspNetCore.SignalR;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ChatViewModel;
using Mohamy.Core.Entity.ChatData;

namespace Mohamy.BusinessLayer.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        // Method to join a chat room and get old messages
        public async Task JoinChat(string senderId, string receiverId)
        {
            string groupName = GetGroupName(senderId, receiverId);

            // Add the user to the group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

            // Retrieve chat history
            IEnumerable<ChatDTO> oldMessages = await _chatService.GetChatsAsync(senderId, receiverId);

            // Send old messages to the user who joined
            await Clients.Group(groupName).SendAsync("ReceiveMessage", oldMessages);
        }

        // Method to send a message
        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            string groupName = GetGroupName(senderId, receiverId);

            // Create a new chat message DTO
            var chatMessage = new ChatDTO
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Message = message,
                SentAt = DateTime.UtcNow
            };

            // Save the message using the service
            ChatDTO savedMessage = await _chatService.SendMessageAsync(chatMessage);

            // Broadcast the message to the group
            await Clients.Group(groupName).SendAsync("ReceiveMessage", savedMessage);
        }

        // Utility method to create a consistent group name
        private string GetGroupName(string user1, string user2)
        {
            return string.CompareOrdinal(user1, user2) < 0 ? $"{user1}-{user2}" : $"{user2}-{user1}";
        }
    }
}
