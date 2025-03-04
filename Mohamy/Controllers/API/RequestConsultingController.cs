﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RequestConsultingController : BaseController, IActionFilter
    {
        private readonly IRequestConsultingService _requestConsultingService;
        private readonly IAccountService _accountService;

        private ApplicationUser? CurrentUser;

        public RequestConsultingController(IRequestConsultingService requestConsultingService, IAccountService accountService)
        {
            _requestConsultingService = requestConsultingService;
            _accountService = accountService;
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

        [Authorize(Policy = "Lawyer")]
        [HttpPost("RequestConsulting")]
        public async Task<ActionResult<BaseResponse>> RequestConsulting([FromBody] RequestConsultingDTO consultingRquest)
        {
            var response = new BaseResponse();

            try
            {
                var success = await _requestConsultingService.AddRequestAsync(consultingRquest);
                response.status = true;
                response.ErrorCode = 200;
                response.Data = success;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"حدث خطأ أثناء تحديث حالة الطلب: {ex.Message}";
            }

            return StatusCode(response.ErrorCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpGet("RequestConsulting")]
        public async Task<ActionResult<BaseResponse>> RequestConsulting([FromQuery] string ConsultingId)
        {
            var response = new BaseResponse();

            try
            {
                var success = await _requestConsultingService.GetRequestsByConsultingAsync(ConsultingId);
                response.status = true;
                response.ErrorCode = 200;
                response.Data = success;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"حدث خطأ أثناء تحديث حالة الطلب: {ex.Message}";
            }

            return StatusCode(response.ErrorCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpPost("Approved")]
        public async Task<ActionResult<BaseResponse>> ApprovedRequestStatus([FromQuery] string requestId)
        {
            var response = new BaseResponse();

            try
            {
                var success = await _requestConsultingService.UpdateRequestStatusAsync(requestId, statusRequestConsulting.Approved);
                response.status = success;

                if (!success)
                {
                    response.ErrorCode = 400;
                    response.ErrorMessage = "تعذر تحديث حالة الطلب";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"حدث خطأ أثناء تحديث حالة الطلب: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpPost("Negotiate")]
        public async Task<ActionResult<BaseResponse>> NegotiateRequestStatus([FromQuery] string requestId)
        {
            var response = new BaseResponse();

            try
            {
                var success = await _requestConsultingService.UpdateRequestStatusAsync(requestId, statusRequestConsulting.Negotiating);
                response.status = success;

                if (!success)
                {
                    response.ErrorCode = 400;
                    response.ErrorMessage = "تعذر تحديث حالة الطلب";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"حدث خطأ أثناء تحديث حالة الطلب: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpPost("Rejected")]
        public async Task<ActionResult<BaseResponse>> RejectedRequestStatus([FromQuery] string requestId)
        {
            var response = new BaseResponse();

            try
            {
                var success = await _requestConsultingService.UpdateRequestStatusAsync(requestId, statusRequestConsulting.Rejection);
                response.status = success;

                if (!success)
                {
                    response.ErrorCode = 400;
                    response.ErrorMessage = "تعذر تحديث حالة الطلب";
                }
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"حدث خطأ أثناء تحديث حالة الطلب: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }
    }
}