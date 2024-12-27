using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Helpers;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Customer")]
    public class ConsultingController : BaseController, IActionFilter
    {
        private readonly IConsultingService _consultingService;
        private readonly IAccountService _accountService;
        private readonly IChatService _chatService;

        private ApplicationUser? CurrentUser;

        public ConsultingController(IAccountService accountService, IConsultingService consultingService, IRequestConsultingService requestConsultingService, IChatService chatService)
        {
            _accountService = accountService;
            _consultingService = consultingService;
            _chatService = chatService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    var user = _accountService.GetUserFromToken(token).Result;
                    CurrentUser = user; // Store the user in the context
                }
                catch (Exception)
                {
                    context.Result = new UnauthorizedResult(); // Early exit if user retrieval fails
                    return;
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }


        [Route("AddConsulting")]
        [HttpPost]
        public async Task<ActionResult<BaseResponse>> AddConsulting([FromForm] ConsultingDTO dto)
        {
            var response = new BaseResponse();

            try
            {
                // Bind the current user ID (ensure CurrentUser is properly set)
                dto.CustomerId = CurrentUser.Id;

                // Call the service to add consulting
                var id = await _consultingService.AddConsultingAsync(dto);

                // Construct a proper response
                response.status = true;
                response.ErrorMessage = "";
                response.Data = new { ConsultingId = id };
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Return a user-friendly error message
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = "An error occurred while adding consulting.";
                response.Data = new { error=ex.Message,
                    InnerException=ex.InnerException.Message
                };
                return StatusCode(response.ErrorCode,response);
            }
        }

        [HttpGet]
        [Route("GetAllConsultingsCustomer")]
        public async Task<ActionResult<BaseResponse>> GetAllConsultingsCustomer()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsByCustomerIdAsync(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetAllConsultingsInProgress")]
        public async Task<ActionResult<BaseResponse>> GetAllConsultingsInProgress()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsInprogress(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetAllConsultingsCompleted")]
        public async Task<ActionResult<BaseResponse>> GetAllConsultingsCompleted()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsCompleted(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetAllConsultingsCancelled")]
        public async Task<ActionResult<BaseResponse>> GetAllConsultingsCancelled()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsCancelled(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetAllServicesInProgress")]
        public async Task<ActionResult<BaseResponse>> GetAllServicesInProgress()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetServicesInprogress(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetAllServicesCompleted")]
        public async Task<ActionResult<BaseResponse>> GetAllServicesCompleted()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsCompleted(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetAllServices")]
        public async Task<ActionResult<BaseResponse>> GetAllServices()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetServices(CurrentUser.Id);

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("GetConsultingById")]
        public async Task<ActionResult<BaseResponse>> GetConsultingById([FromQuery]string id)
        {
            var response = new BaseResponse();

            try
            {
                var consulting = await _consultingService.GetConsultingByIdAsync(id);
                if (consulting == null)
                {
                    response.status = false;
                    response.ErrorCode = 404;
                    response.ErrorMessage = "Consulting not found.";
                    return NotFound(response);
                }

                response.status = true;
                response.Data = consulting;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving consulting: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpPost]
        [Route("CancelConsulting")]
        public async Task<ActionResult<BaseResponse>> CancelConsulting([FromQuery] string id)
        {
            var response = new BaseResponse();

            try
            {
                await _consultingService.UpdateConsultingStatusAsync(id, statusConsulting.Cancelled);
                response.status = true;
                response.Data = "تم الغاء الاستشارة بنجاح";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while canceling consulting: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpPost]
        [Route("PaymentConsulting")]
        public async Task<ActionResult<BaseResponse>> PaymentConsulting([FromQuery] string id)
        {
            var response = new BaseResponse();

            try
            {
                await _consultingService.UpdateConsultingStatusAsync(id, statusConsulting.InProgress);
                response.status = true;
                response.Data = "تم دفع الاستشارة بنجاح";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while payment consulting: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpPost]
        [Route("FinishConsulting")]
        public async Task<ActionResult<BaseResponse>> FinishConsulting([FromQuery] string id)
        {
            var response = new BaseResponse();

            try
            {
                await _consultingService.UpdateConsultingStatusAsync(id, statusConsulting.Completed);
                response.status = true;
                response.Data = "تم انتهاء الاستشارة بنجاح";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while Finish consulting: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("Files")]
        public async Task<ActionResult<BaseResponse>> GetAllFiles([FromQuery] string senderId, [FromQuery] string receiveId)
        {
            var response = new BaseResponse();

            try
            {
                var Files = _chatService.GetAllFiles(senderId,receiveId);
                if (Files == null)
                {
                    response.status = false;
                    response.ErrorCode = 404;
                    response.ErrorMessage = "Files not found.";
                    return NotFound(response);
                }

                response.status = true;
                response.Data = Files;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving Files: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet]
        [Route("Images")]
        public async Task<ActionResult<BaseResponse>> GetAllImages([FromQuery] string senderId, [FromQuery] string receiveId)
        {
            var response = new BaseResponse();

            try
            {
                var Files = _chatService.GetAllImages(senderId,receiveId);
                if (Files == null)
                {
                    response.status = false;
                    response.ErrorCode = 404;
                    response.ErrorMessage = "Files not found.";
                    return NotFound(response);
                }

                response.status = true;
                response.Data = Files;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving Files: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }
    }
}
