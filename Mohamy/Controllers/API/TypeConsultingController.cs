using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;

namespace Mohamy.Controllers.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TypeConsultingController : BaseController
    {
        private readonly IMainConsultingService _mainConsultingService;
        private readonly ISubConsultingService _subConsultingService;
        private readonly IAccountService _accountService;

        public TypeConsultingController(IMainConsultingService mainConsultingService, ISubConsultingService subConsultingService, IAccountService accountService)
        {
            _mainConsultingService = mainConsultingService;
            _subConsultingService = subConsultingService;
            _accountService = accountService;
        }

        // GET: api/Consulting/MainConsultings
        [Authorize(Policy = "Customer")]
        [HttpGet("MainConsultings")]
        public async Task<ActionResult<BaseResponse>> GetAllMainConsultings()
        {
            var response = new BaseResponse();
            try
            {
                var mainConsultings = await _mainConsultingService.GetAllAsync();
                response.status = true;
                response.ErrorCode = 200;
                response.Data = mainConsultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving main consultings: {ex.Message}";
            }

            return StatusCode(response.ErrorCode, response);
        }

        // GET: api/Consulting/MainConsultings
        [Authorize(Policy = "Customer")]
        [HttpGet("MainConsultingsById")]
        public async Task<ActionResult<BaseResponse>> GetMainConsultingsID([FromQuery]string MainConsultingId)
        {
            var response = new BaseResponse();

            try
            {
                var mainConsultings = await _mainConsultingService.GetByIdAsync(MainConsultingId);
                response.status = true;
                response.Data = mainConsultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving main consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        // GET: api/Consulting/SubConsultings
        [HttpGet("SubConsultings")]
        public async Task<ActionResult<BaseResponse>> GetAllSubConsultings()
        {
            var response = new BaseResponse();

            try
            {
                var subConsultings = await _subConsultingService.GetAllAsync();
                response.status = true;
                response.Data = subConsultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving sub consultings: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpGet("subconsulting")]

        public async Task<ActionResult<BaseResponse>> GetUsersBySubConsulting([FromQuery] string subConsultingId)
        {
            var response = new BaseResponse();

            if (string.IsNullOrEmpty(subConsultingId))
            {
                response.status = false;
                response.ErrorCode = 400;
                response.ErrorMessage = "SubConsultingId cannot be null or empty.";
                return BadRequest(response);
            }

            try
            {
                var users = await _subConsultingService.GetUsersBySubConsultingAsync(subConsultingId);

                if (users == null || !users.Any())
                {
                    response.status = false;
                    response.ErrorCode = 404;
                    response.ErrorMessage = "No users found for the specified subconsulting.";
                    return NotFound(response);
                }

                var usersDto = users.Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.PhoneNumber,
                    ProfileUrl = _accountService.GetUserProfileImage(u.Profile.Id).Result,
                    u.Description,
                    u.yearsExperience,
                    u.City,
                    u.Region,
                    u.PriceService,
                    u.Online,
                    numberConsulting = 0,
                    rating = 0,
                });

                response.status = true;
                response.Data = usersDto;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving users: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [Authorize(Policy = "Customer")]
        [HttpGet("subconsultingUser")]
        public async Task<ActionResult<BaseResponse>> GetSubConsultingByUser([FromQuery] string UserId)
        {
            var response = new BaseResponse();

            if (string.IsNullOrEmpty(UserId))
            {
                response.status = false;
                response.ErrorCode = 400;
                response.ErrorMessage = "UserId cannot be null or empty.";
                return BadRequest(response);
            }

            try
            {
                var SubConsultings = await _subConsultingService.GetSubConsultingByUsersAsync(UserId);

                if (SubConsultings == null || !SubConsultings.Any())
                {
                    response.status = false;
                    response.ErrorCode = 404;
                    response.ErrorMessage = "Not found the specified subconsulting to User.";
                    return NotFound(response);
                }
                response.status = true;
                response.Data = SubConsultings;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving users: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpGet("subconsultingByMainConsulting")]
        public async Task<ActionResult<BaseResponse>> subconsultingByMain([FromQuery] string mainConsultingId)
        {
            var response = new BaseResponse();

            if (string.IsNullOrEmpty(mainConsultingId))
            {
                response.status = false;
                response.ErrorCode = 400;
                response.ErrorMessage = "SubConsultingId cannot be null or empty.";
                return BadRequest(response);
            }

            try
            {
                var subConsulting = await _subConsultingService.GetSubConsultingByMainAsync(mainConsultingId);

                if (subConsulting == null || !subConsulting.Any())
                {
                    response.status = false;
                    response.ErrorCode = 404;
                    response.ErrorMessage = "No subConsulting found for the specified Main Consulting.";
                    return NotFound(response);
                }

                response.status = true;
                response.Data = subConsulting;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while retrieving users: {ex.Message}";
            }

            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

    }
}
