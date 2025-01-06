using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.CommunicationViewModel;
using Agora;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Mohamy.BusinessLayer.Services
{
    public class CommunicationSerivce : ICommunicationService
    {
        private readonly AgoraConfigurations _agoraConfig;
        public CommunicationSerivce(IOptions<AgoraConfigurations> agoraConfig) {
            _agoraConfig = agoraConfig.Value;
        }

        public TokenResponseDTO GenerateToken(TokenRequestDTO tokenRequest) 
        {
            TokenResponseDTO response = new TokenResponseDTO();
            response.channelName = GetChannelName(tokenRequest.senderId, tokenRequest.receiverId);
            response.userId = GenerateUserId();
            uint privilegeExpiredTs = (uint)_agoraConfig.TokenExpiry + (uint)Utils.getTimestamp();
            AccessToken accessToken = new AccessToken(_agoraConfig.AppId, _agoraConfig.AppCertificate, response.channelName, response.userId);
            accessToken.addPrivilege(Privileges.kJoinChannel, privilegeExpiredTs);
            accessToken.addPrivilege(Privileges.kPublishAudioStream, privilegeExpiredTs);
            accessToken.addPrivilege(Privileges.kPublishVideoStream, privilegeExpiredTs);
            accessToken.addPrivilege(Privileges.kPublishDataStream, privilegeExpiredTs);
            response.token = accessToken.build();

            return response;
        }

        private string GetChannelName(string user1, string user2)
        {
            string ids = string.CompareOrdinal(user1, user2) < 0 ? $"{user1}-{user2}" : $"{user2}-{user1}";
            return Regex.Replace(ids, "[^a-zA-Z0-9]", "");
        }

        private static string GenerateUserId()
        {
            Random random = new Random();
            return random.Next(1, 10000).ToString();
        }
    }
}

