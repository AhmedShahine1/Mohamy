using Microsoft.EntityFrameworkCore;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.ChatViewModel;
using Mohamy.Core.Entity.ChatData;
using Mohamy.RepositoryLayer.Interfaces;

namespace Mohamy.BusinessLayer.Services
{
    public class ChatService : IChatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileHandling _fileHandling;
        private readonly IAccountService _accountService;

        public ChatService(IUnitOfWork unitOfWork, IFileHandling fileHandling, IAccountService accountService)
        {
            _unitOfWork = unitOfWork;
            _fileHandling = fileHandling;
            _accountService = accountService;
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
                SentAt = m.CreatedAt
            });
        }

        public async Task<ChatDTO> SendMessageAsync(ChatDTO messageDTO)
        {
            var message = new Chat
            {
                SenderId = messageDTO.SenderId,
                ReceiverId = messageDTO.ReceiverId,
                Message = messageDTO.Message
            };
            if (messageDTO.File != null)
            {
                var path = await _accountService.GetPathByName("ChatFiles");

                    message.ImagesId = await _fileHandling.UploadFile(messageDTO.File, path);
            }
            await _unitOfWork.ChatRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            return new ChatDTO
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                FileUrl = message.ImagesId is not null ? await _fileHandling.GetFile(message.ImagesId) : null,
                Message = message.Message,
                SentAt = message.CreatedAt
            };
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
