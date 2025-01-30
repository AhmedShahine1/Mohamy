using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.CommunicationViewModel;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommunicationController : BaseController
    {
        private readonly ICommunicationService _communicationService;
        public CommunicationController(ICommunicationService communicationService)
        {
            _communicationService = communicationService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> GenerateToken([FromBody] TokenRequestDTO tokenRequest)
        {
            var response = new BaseResponse();

            try
            {
                TokenResponseDTO tokenResponse = _communicationService.GenerateToken(tokenRequest);

                response.status = true;
                response.Data = tokenResponse;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"حدث خطأ أثناء إنشاء الرمز: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }
    }
}