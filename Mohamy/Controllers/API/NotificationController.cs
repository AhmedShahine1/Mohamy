using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetNotificationsAsync([FromQuery]string userId)
        {
            var response = new BaseResponse();
            try
            {
                var notifications = await _notificationService.GetNotificationsAsync(userId);
                response.status = true;
                response.Data = notifications;
                response.ErrorMessage = string.Empty;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = ex.Message;
                return StatusCode(500, response);
            }
        }
    }
}
