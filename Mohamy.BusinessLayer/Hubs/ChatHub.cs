using Microsoft.AspNetCore.SignalR;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ChatViewModel;

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
        public async Task JoinChat(string senderId, string receiverId, string consultingId)
        {
            string chatGroup = GetGroupName(senderId, receiverId, consultingId);

            await Groups.AddToGroupAsync(Context.ConnectionId, chatGroup);

            // Retrieve chat history between the sender and receiver
            var chatHistory = await _chatService.GetChatsAsync(senderId, receiverId, consultingId);

            // Send chat history to the joining user
            await Clients.Group(chatGroup).SendAsync("ChatHistory", chatHistory);
        }

        public async Task ReadMessages(string senderId, string receiverId, string consultingId)
        {
            var messageIds = await _chatService.ReadMessages(senderId, receiverId, consultingId);
            if (messageIds.Count > 0)
            {
                string chatGroup = GetGroupName(senderId, receiverId, consultingId);
                await Clients.Group(chatGroup).SendAsync("MessageReads", messageIds);
            }
        }

        // Method to send a message
        public async Task SendMessage(ChatDTO chatMessage)
        {
            string chatGroup = GetGroupName(chatMessage.SenderId, chatMessage.ReceiverId, chatMessage.ConsultingId);

            chatMessage.SentAt = DateTime.UtcNow;

            // Save the message to the database
            await _chatService.SendMessageAsync(chatMessage);

            // Broadcast the message to all clients in the chat group
            await Clients.Group(chatGroup).SendAsync("ReceiveMessage", chatMessage);
        }

        // Utility method to create a consistent group name
        private string GetGroupName(string user1, string user2, string consultingId)
        {
            string groupName = string.CompareOrdinal(user1, user2) < 0 ? $"{user1}-{user2}" : $"{user2}-{user1}";
            return $"{groupName}-{consultingId}";
        }
    }
}
