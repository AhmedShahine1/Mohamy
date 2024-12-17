using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.AuthViewModel.UpdateModel;

namespace Mohamy.Controllers.API
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer([FromForm] RegisterCustomer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Invalid model"
                });
            }

            try
            {
                var result = await _accountService.RegisterCustomer(model);

                if (result.Succeeded)
                {
                    var modelR = new LoginModel
                    {
                        PhoneNumber = model.PhoneNumber,
                        Password = "Ahmed@123"
                    };

                    var resultLogin = await _accountService.Login(modelR);

                    if (resultLogin.IsSuccess)
                    {
                        var user = await _accountService.GetUserFromToken(resultLogin.Token);
                        var authDto = _mapper.Map<AuthDTO>(user);
                        authDto.Token = resultLogin.Token;
                        authDto.ProfileImage = await _accountService.GetUserProfileImage(user.ProfileId);

                        return Ok(new BaseResponse
                        {
                            status = true,
                            Data = authDto
                        });
                    }

                    return Unauthorized(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 401,
                        ErrorMessage = resultLogin.ErrorMessage
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "User registration failed.",
                    Data = result.Errors.Select(e => e.Description).ToArray()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = ex.Message
                });
            }
        }

        [HttpGet("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomerDetails()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var customer = await _accountService.GetUserFromToken(token);
                if (customer == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "Customer not found"
                    });
                }

                var authDto = _mapper.Map<AuthDTO>(customer);
                authDto.ProfileImage= await _accountService.GetUserProfileImage(customer.ProfileId);
                return Ok(new BaseResponse
                {
                    status = true,
                    Data = authDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("logout")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var user = await _accountService.GetUserFromToken(token);
                var isSuccess = await _accountService.Logout(user);

                if (isSuccess)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "Successfully logged out"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Logout failed"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateCustomer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Customer")]
        public async Task<IActionResult> UpdateCustomer([FromForm] UpdateCustomer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Invalid model"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var customer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateCustomer(customer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "User updated successfully"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "User update failed",
                    Data = result.Errors.Select(e => e.Description).ToArray()
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new BaseResponse
                {
                    status = false,
                    ErrorCode = 404,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ConfirmPhoneNumber([FromQuery] string customerPhoneNumber, [FromQuery] string OTP)
        {
            if (string.IsNullOrEmpty(customerPhoneNumber))
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Customer Phone Number cannot be null or empty"
                });
            }

            try
            {
                var result = await _accountService.ValidateOTP(customerPhoneNumber,OTP);

                if (result)
                {
                    var model = new LoginModel
                    {
                        PhoneNumber = customerPhoneNumber,
                        Password = "Ahmed@123"
                    };
                    var resultLogin = await _accountService.Login(model);

                    if (resultLogin.IsSuccess)
                    {
                        var user = await _accountService.GetUserFromToken(resultLogin.Token);
                        var authDto = _mapper.Map<AuthDTO>(user);
                        authDto.Token = resultLogin.Token;
                        authDto.ProfileImage = await _accountService.GetUserProfileImage(user.ProfileId);

                        return Ok(new BaseResponse
                        {
                            status = true,
                            Data = authDto
                        });
                    }

                    return Unauthorized(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 401,
                        ErrorMessage = resultLogin.ErrorMessage
                    });

                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Failed to confirm phone number.",
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new BaseResponse
                {
                    status = false,
                    ErrorCode = 404,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP([FromQuery]string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Customer Phone Number cannot be null or empty"
                });
            }

            try
            {
                var result = await _accountService.SendOTP(PhoneNumber);

                if (result)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = new { Message = "OTP send successfully." }
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Failed to send OTP to phone number.",
                });
            }
            catch (ArgumentException ex)
            {
                return NotFound(new BaseResponse
                {
                    status = false,
                    ErrorCode = 404,
                    ErrorMessage = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("Lawyer/{LawyerId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Customer")]
        public async Task<IActionResult> GetLaywer(string LawyerId)
        {
            try
            {
                var Laywer = await _accountService.GetUserById(LawyerId);

                if (Laywer == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "Lawyer not found"
                    });
                }
                var authDto = _mapper.Map<AuthDTO>(Laywer);
                authDto.ProfileImage = await _accountService.GetUserProfileImage(Laywer.ProfileId);
                //authDto.ExperienceNames = _experienceService.GetAllExperiencesByUserIdAsync(LawyerId)
                //    .Result
                //    .Select(s => s.subConsulting.Name)
                //    .ToList();

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = authDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteAccount()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                var customer = await _accountService.GetUserFromToken(token);

                if (customer == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "Customer not found"
                    });
                }

                var result = await _accountService.Suspend(customer.Id);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "User account deleted successfully."
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "Failed to delete account",
                    Data = result.Errors.Select(e => e.Description).ToArray()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "An unexpected error occurred.",
                    Data = ex.Message
                });
            }
        }
    }
}
