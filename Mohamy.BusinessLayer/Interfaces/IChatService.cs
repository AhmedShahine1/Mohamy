using Mohamy.Core.DTO.ChatViewModel;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IChatService
    {
        Task<List<string>> GetAllFiles(string senderId, string receiverId);
        Task<List<string>> GetAllImages(string senderId, string receiverId);
        Task<IEnumerable<ChatDTO>> GetChatsAsync(string senderId, string receiverId);
        Task<ChatDTO> SendMessageAsync(ChatDTO messageDTO);
        Task<IList<MessageIdDTO>> ReadMessages(string senderId, string receiverId);
    }
}
