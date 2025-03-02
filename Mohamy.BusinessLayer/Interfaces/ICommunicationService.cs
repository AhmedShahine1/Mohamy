using Mohamy.Core.DTO.CommunicationViewModel;

namespace Mohamy.BusinessLayer.Interfaces
{
    public interface ICommunicationService
    {
        public TokenResponseDTO GenerateToken(TokenRequestDTO tokenRequest);
        public Task SendCallNotificationAsync(TokenRequestDTO tokenRequest);

    }
}
