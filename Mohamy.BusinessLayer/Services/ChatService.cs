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

        public ChatService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ChatDTO>> GetChatsAsync(string senderId, string receiverId)
        {
            var messages = await _unitOfWork.ChatRepository.FindAllAsync(
                m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                     (m.SenderId == receiverId && m.ReceiverId == senderId),
                include: q=>q.Include(i=>i.Sender)
                .Include(i=>i.Receiver));

            return messages.Select(m => new ChatDTO
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
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

            await _unitOfWork.ChatRepository.AddAsync(message);
            await _unitOfWork.SaveChangesAsync();

            return new ChatDTO
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Message = message.Message,
                SentAt = message.CreatedAt
            };
        }
    }

}
