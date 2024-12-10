using Mohamy.Core.DTO.ChatViewModel;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface IChatService
    {
        Task<IEnumerable<ChatDTO>> GetChatsAsync(string senderId, string receiverId);
        Task<ChatDTO> SendMessageAsync(ChatDTO messageDTO);
    }

}
