using Azure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.BusinessLayer.Services;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.ChatViewModel;
using Mohamy.Core.DTO.ConsultingViewModel;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.ConsultingData;
using Mohamy.Core.Helpers;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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

        [Authorize(Policy = "Customer")]
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
        public async Task<ActionResult<BaseResponse>> GetAllFiles([FromQuery] string senderId, [FromQuery] string receiverId)
        {
            var response = new BaseResponse();

            try
            {
                var Files =await _chatService.GetAllFiles(senderId, receiverId);
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
        public async Task<ActionResult<BaseResponse>> GetAllImages([FromQuery] string senderId, [FromQuery] string receiverId)
        {
            var response = new BaseResponse();

            try
            {
                var Files = await _chatService.GetAllImages(senderId, receiverId);
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

        [HttpPost("SendMessage")]
        public async Task<IActionResult> SendMessage([FromForm] ChatDTO chatDTO)
        {
            if (string.IsNullOrEmpty(chatDTO.SenderId) || string.IsNullOrEmpty(chatDTO.ReceiverId))
                return BadRequest("SenderId and ReceiverId are required.");

            try
            {
                var result = await _chatService.SendMessageAsync(chatDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception if a logging system is in place
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [Authorize(Policy = "Lawyer")]
        [HttpGet]
        [Route("GetAvailableConsultations")]
        public async Task<ActionResult<BaseResponse>> GetAvailableConsultations()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetAvailableConsultations(CurrentUser.Id);

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

        [Authorize(Policy = "Lawyer")]
        [HttpPost]
        [Route("AcceptConsultation")]
        public async Task<ActionResult<BaseResponse>> AcceptConsultation([FromQuery] string id)
        {
            var response = new BaseResponse();

            try
            {
                await _consultingService.AcceptConsultation(CurrentUser.Id, id);

                response.status = true;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while accepting consultation: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [Authorize(Policy = "Lawyer")]
        [HttpGet]
        [Route("GetAvailableServices")]
        public async Task<ActionResult<BaseResponse>> GetAvailableServices()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetAvailableServices();

                response.status = true;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving services: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [Authorize(Policy = "Lawyer")]
        [HttpGet]
        [Route("GetLawyerConsultingsCompleted")]
        public async Task<ActionResult<BaseResponse>> GetLawyerConsultingsCompleted()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsCompleted(CurrentUser.Id, true);

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

        [Authorize(Policy = "Lawyer")]
        [HttpGet]
        [Route("GetLawyerConsultingsInProgress")]
        public async Task<ActionResult<BaseResponse>> GetLawyerConsultingsInProgress()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetConsultingsInprogress(CurrentUser.Id, true);

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

        [Authorize(Policy = "Lawyer")]
        [HttpGet("GetInProgressRequestConsultings")]
        public async Task<ActionResult<BaseResponse>> GetInProgressRequestConsultings()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetRequestConsultings(CurrentUser.Id, statusConsulting.InProgress);
                response.status = true;
                response.ErrorCode = 200;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while updating request status: {ex.Message}";
            }

            return StatusCode(response.ErrorCode, response);
        }

        [Authorize(Policy = "Lawyer")]
        [HttpGet("GetCompletedRequestConsultings")]
        public async Task<ActionResult<BaseResponse>> GetCompletedRequestConsultings()
        {
            var response = new BaseResponse();

            try
            {
                var consultings = await _consultingService.GetRequestConsultings(CurrentUser.Id, statusConsulting.Completed);
                response.status = true;
                response.ErrorCode = 200;
                response.Data = consultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while updating request status: {ex.Message}";
            }

            return StatusCode(response.ErrorCode, response);
        }

        [Authorize(Policy = "Lawyer")]
        [HttpPost("Ignore")]
        public async Task<IActionResult> IgnoreConsultationAsync([FromQuery] string consultingId)
        {
            var response = new BaseResponse();
            try
            {
                await _consultingService.IgnoreConsultationAsync(CurrentUser.Id, consultingId);
                response.status = true;
                response.ErrorCode = 200;
                response.Data = "Consulting ignored successfully";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while ignoring consulting: {ex.Message}";
            }

            return StatusCode(response.ErrorCode, response);
        }


    }
}
