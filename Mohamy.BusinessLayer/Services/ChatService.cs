using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Hubs;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ChatViewModel;
using Mohamy.Core.DTO.NotificationViewModel;
using Mohamy.Core.Entity.ChatData;
using Mohamy.Core.Helpers;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IAccountService _accountService;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotificationService _notificationService;

        public ChatService(IUnitOfWork unitOfWork, IFileHandling fileHandling, INotificationService notificationService, IAccountService accountService, IHubContext<ChatHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _fileHandling = fileHandling;
            _accountService = accountService;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        public async Task<IEnumerable<ChatDTO>> GetChatsAsync(string senderId, string receiverId)
        {
            var messages = await _unitOfWork.ChatRepository.FindAllAsync(
                m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                (m.SenderId == receiverId && m.ReceiverId == senderId),
                include: q => q.Include(i => i.Sender)
                   .Include(i => i.Receiver)
                   .Include(i => i.Images),
                orderBy: q => q.OrderByDescending(m => m.CreatedAt));

            return messages.Select(m => new ChatDTO
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                FileUrl=  m.Images is not null ? _fileHandling.GetFile(m.ImagesId).Result:null,
                Message = m.Message,
                SentAt = m.CreatedAt,
                IsRead = m.IsRead
            });
        }

        public async Task<IList<MessageIdDTO>> ReadMessages(string senderId, string receiverId)
        {
            IList<MessageIdDTO> messageIds = new List<MessageIdDTO>();

            var messages = await _unitOfWork.ChatRepository.FindAllAsync(
                m => m.SenderId == receiverId && m.ReceiverId == senderId && m.IsRead == false);

            if (messages.Any()) {
                foreach (var message in messages)
                {
                    messageIds.Add(new MessageIdDTO() { Id = message.Id });
                    message.IsRead = true;
                    message.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.ChatRepository.Update(message);
                }
                await _unitOfWork.SaveChangesAsync();
            }

            return messageIds;
        }

        public async Task<ChatDTO> SendMessageAsync(ChatDTO messageDTO)
        {
            // Create a new chat message
            var message = new Chat
            {
                SenderId = messageDTO.SenderId,
                ReceiverId = messageDTO.ReceiverId,
                Message = messageDTO.Message,
                CreatedAt = DateTime.UtcNow
            };
            // Handle file if it exists
            if (messageDTO.File != null)
            {
                var path = await _accountService.GetPathByName("ChatFiles");
                message.ImagesId = await _fileHandling.UploadFile(messageDTO.File, path);
                messageDTO.FileUrl = await _fileHandling.GetFile(message.ImagesId);
                // Determine the group name
                string chatGroup = GetGroupName(messageDTO.SenderId, messageDTO.ReceiverId);

                // Broadcast the message to the SignalR group
                await _hubContext.Clients.Group(chatGroup).SendAsync("ReceiveMessage", messageDTO);
            }
            // Prepare the DTO for broadcasting
            var chatDTO = new ChatDTO
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                FileUrl = messageDTO.FileUrl,
                Message = message.Message,
                SentAt = message.CreatedAt
            };

            // Save the chat message in the repository
            await _unitOfWork.ChatRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            
            await _notificationService.SaveNotificationAsync(new SaveNotificationDTO
            {
                UserId = message.ReceiverId,
                NotificationType = NotificationType.Message,
                ActionId = message.SenderId
            });

            return chatDTO;
        }

        private string GetGroupName(string user1, string user2)
        {
            return string.CompareOrdinal(user1, user2) < 0 ? $"{user1}-{user2}" : $"{user2}-{user1}";
        }

        public async Task<List<string>> GetAllImages(string senderId, string receiverId)
        {
            var messages = await _unitOfWork.ChatRepository.FindAllAsync(
                m => ((m.SenderId == senderId && m.ReceiverId == receiverId) ||
                      (m.SenderId == receiverId && m.ReceiverId == senderId)) && m.ImagesId != null,
                include: q => q.Include(i => i.Images));

            var imageFiles = messages
                .Select(m => _fileHandling.GetFile(m.ImagesId).Result)
                .Where(file => IsImageFile(file)) // Filter to include only image files
                .ToList();

            return imageFiles;
        }

        public async Task<List<string>> GetAllFiles(string senderId, string receiverId)
        {
            var messages = await _unitOfWork.ChatRepository.FindAllAsync(
                m => ((m.SenderId == senderId && m.ReceiverId == receiverId) ||
                      (m.SenderId == receiverId && m.ReceiverId == senderId)) && m.ImagesId != null,
                include: q => q.Include(i => i.Images));

            var imageFiles = messages
                .Select(m => _fileHandling.GetFile(m.ImagesId).Result)
                .Where(file => !IsImageFile(file)) // Filter to include only image files
                .ToList();

            return imageFiles;
        }

        private bool IsImageFile(string filePath)
        {
            var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff" };
            return imageExtensions.Any(ext => filePath.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
        }
    }
}
