using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.AuthViewModel;
using Mohamy.Core.DTO.AuthViewModel.LawyerDetailsModel;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.DTO.AuthViewModel.UpdateModel;
using Mohamy.Core.DTO.NotificationViewModel;

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
                    ErrorMessage = "نموذج غير صالح"
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
                    ErrorMessage = "فشل تسجيل المستخدم",
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
                        ErrorMessage = "لم يتم العثور على العميل"
                    });
                }

                var authDto = _mapper.Map<AuthDTO>(customer);
                var specialties = await _accountService.GetAllSpecialtiesAsync(authDto.Id);
                authDto.Specialties = specialties.Select(s => new SpecialtiesDTO
                {
                    Id = s.mainConsultingId,
                    mainConsultingName = s.mainConsulting.Name
                }).ToList();

                var professions = await _accountService.GetAllProfessionsAsync(authDto.Id);
                authDto.Professions = professions.Select(p => new ProfessionDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                }).ToList();

                var licenses = await _accountService.GetAllLawyerLicensesAsync(authDto.Id);
                if (licenses.Any())
                {
                    var primaryLicense = licenses.FirstOrDefault();
                    authDto.lawyerLicenseId = primaryLicense.Id;
                    authDto.lawyerLicenseNumber = primaryLicense.LicenseNumber;
                }

                authDto.ProfileImage = await _accountService.GetUserProfileImage(customer.ProfileId);
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }


        [HttpGet("UserNotificationProfileData")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUserNotificationProfileDataAsync([FromQuery] string userId)
        {
            try
            {
                var user = await _accountService.GetUserNotificationProfileDataAsync(userId);
                return Ok(new BaseResponse
                {
                    status = true,
                    Data = user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
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
                        Data = "تم تسجيل الخروج بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "فشل تسجيل الخروج"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
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
                    ErrorMessage = "نموذج غير صالح"
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
                        Data = "تم تحديث المستخدم بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المستخدم",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
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
                    ErrorMessage = "لا يمكن أن يكون رقم هاتف العميل خاليًا أو فارغًا"
                });
            }

            try
            {
                var result = await _accountService.ValidateOTP(customerPhoneNumber, OTP);

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
                    ErrorMessage = "الكود الذي ادخلته غير صحيح",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("SendOTP")]
        public async Task<IActionResult> SendOTP([FromQuery] string PhoneNumber)
        {
            if (string.IsNullOrEmpty(PhoneNumber))
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "لا يمكن أن يكون رقم هاتف العميل خاليًا أو فارغًا"
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
                        Data = new { Message = "تم إرسال OTP بنجاح" }
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "فشل الإرسال إلى رقم الهاتف",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
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
                var lawyer = await _accountService.GetUserById(LawyerId);

                if (lawyer == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "لم يتم العثور على المحامي"
                    });
                }

                // Map basic lawyer details to AuthDTO
                var authDto = _mapper.Map<AuthDTO>(lawyer);

                // Populate ProfileImage
                authDto.ProfileImage = await _accountService.GetUserProfileImage(lawyer.ProfileId);

                // Populate specialties
                var specialties = await _accountService.GetAllSpecialtiesAsync(LawyerId);
                authDto.Specialties = specialties.Select(s => new SpecialtiesDTO
                {
                    Id = s.Id,
                    mainConsultingName = s.mainConsulting.Name
                }).ToList();

                // Populate experiences
                var experiences = await _accountService.GetAllExperiencesAsync(LawyerId);
                authDto.Experiences = experiences.Select(e => new ExperienceDTO
                {
                    Id = e.Id,
                    Start = e.Start,
                    End = e.End,
                    Country = e.Country,
                    Description = e.Description,
                    subConsultingName = e.subConsulting.Name
                }).ToList();

                // Populate lawyer licenses
                var licenses = await _accountService.GetAllLawyerLicensesAsync(LawyerId);
                if (licenses.Any())
                {
                    var primaryLicense = licenses.FirstOrDefault(); // Assuming only one primary license
                    authDto.lawyerLicenseId = primaryLicense.Id;
                    authDto.lawyerLicenseNumber = primaryLicense.LicenseNumber;
                    authDto.lawyerLicenseState = primaryLicense.State;
                    authDto.lawyerLicenseStart = primaryLicense.Start;
                    authDto.lawyerLicenseEnd = primaryLicense.End;
                }

                // Populate graduation certificates
                var certificates = await _accountService.GetAllGraduationCertificatesAsync(LawyerId);
                authDto.GraduationCertificates = certificates.Select(c => new GraduationCertificateDTO
                {
                    Id = c.Id,
                    Start = c.Start,
                    End = c.End,
                    Country = c.Country,
                    Collage = c.Collage,
                    University = c.University,
                    Description = c.Description
                }).ToList();

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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetLawyers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Customer")]
        public async Task<IActionResult> SearchLawyers(
       [FromQuery] string? keyword,
       [FromQuery] string? city,
       [FromQuery] string? specialization,
       [FromQuery] int? minYearsExperience,
       [FromQuery] int? maxYearsExperience,
       [FromQuery] string? sortBy)
        {
            try
            {
                var lawyers = await _accountService.GetLawyersAsync(
                    keyword, city, specialization, minYearsExperience, maxYearsExperience, sortBy);

                if (!lawyers.Any())
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "لم يتم العثور على محامين مطابقين للمعايير"
                    });

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = lawyers
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
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
                var user = await _accountService.GetUserFromToken(token);

                if (user == null)
                {
                    return NotFound(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 404,
                        ErrorMessage = "لم يتم العثور على المستخدم"
                    });
                }

                var result = await _accountService.DeleteAccountAsync(user.Id);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم حذف حساب المستخدم بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "فشل في حذف الحساب",
                    Data = result.Errors.Select(e => e.Description).ToArray()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateProfileImage")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateProfileImageAsync([FromForm] UpdateProfileImage model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var user = await _accountService.GetUserFromToken(token);
                await _accountService.UpdateProfileImageAsync(user.Id, model);

                return Ok(new BaseResponse
                {
                    status = true,
                    Data = "تم تحديث الصورة الشخصية بنجاح"
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }



        [HttpPost("RegisterLawyer")]
        public async Task<IActionResult> RegisterLawyer([FromForm] RegisterLawyer model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 400,
                        ErrorMessage = "نموذج غير صالح"
                    });
                }

                var result = await _accountService.RegisterLawyer(model);

                if (result.result.Succeeded)
                {
                    var response = new BaseResponse();
                    response.status = true;
                    response.ErrorMessage = "";
                    response.Data = result.userId;
                    return Ok(response);
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تسجيل المحامي",
                    Data = result.result.Errors.Select(e => e.Description).ToArray()
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

        [HttpPost("LawyerLogin")]
        public async Task<IActionResult> LawyerLogin([FromBody] LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 400,
                        ErrorMessage = "نموذج غير صالح"
                    });
                }
                
                var loginModel = new LoginModel
                {
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password
                };
                
                var resultLogin = await _accountService.LawyerLogin(model);

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

                    return BadRequest(new BaseResponse
                    {
                        status = false,
                        ErrorCode = 401,
                        ErrorMessage = resultLogin.ErrorMessage,
                    });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("ChangeLawyerRegistrationStatus")]
        public async Task<IActionResult> ChangeLawyerRegistrationStatus([FromQuery] string lawyerId)
        {
            try
            {
                var resultLogin = await _accountService.ChangeLawyerRegistrationStatus(lawyerId);

                if (resultLogin.IsSuccess)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = resultLogin.Message
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

       
        [HttpPost("UpdateLawyer")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyer([FromBody] UpdateLawyer model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyer(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المحامي بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المحامي",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateLawyerProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerProfile([FromBody] UpdateLawyerProfile model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyerProfile(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المحامي بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المحامي",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateLawyerLanguages")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerLanguages([FromBody] UpdateLanguages model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyerLanguages(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "Language updated successfully"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "Language update failed",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("ChangeLawyerAvailability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> ChangeLawyerAvailability()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.ChangeLawyerAvailability(lawyer.Id);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "Availability updated successfully"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "Availability update failed",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateLawyerPassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerPassword(UpdatePassword model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdatePasswordAsync(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "Passowrd updated successfully"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "Passowrd update failed",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateLawyerPhone")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerPhone([FromBody] UpdatePhone model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyerPhone(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المحامي بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المحامي",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpDelete("DeleteProfession")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> DeleteProfessionAsync([FromQuery] string professionId)
        {
            var response = new BaseResponse();
            try
            {
                await _accountService.DeleteProfessionAsync(professionId);
                response.status = true;
            }
            catch (Exception ex)
            {
                response.status = false;
                response.ErrorCode = 500;
                response.ErrorMessage = $"An error occurred while deleting profession: {ex.Message}";
            }
            return StatusCode(response.status ? 200 : response.ErrorCode, response);
        }

        [HttpPost("UpdateLawyerBank")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerBank([FromBody] UpdateBank model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyerBank(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المحامي بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المحامي",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateLawyerPrice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerPrice([FromBody] UpdatePrice model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyerPrice(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المحامي بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المحامي",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("UpdateLawyerSpecialities")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Lawyer")]
        public async Task<IActionResult> UpdateLawyerSpecialities([FromBody] UpdateSpecialities model)
        {
            if (model.mainConsultingId.Count == 0)
            {
                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 400,
                    ErrorMessage = "نموذج غير صالح"
                });
            }

            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.UpdateLawyerSpecialities(lawyer.Id, model);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث التخصصات بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث التخصصات",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("SetUserOnline")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SetUserOnlineAsync()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.SetUserOnlineOfflineStatusAsync(lawyer.Id, true);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المستخدم بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المستخدم",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("SetUserOffline")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SetUserOfflineAsync()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var lawyer = await _accountService.GetUserFromToken(token);
                var result = await _accountService.SetUserOnlineOfflineStatusAsync(lawyer.Id, false);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم تحديث المستخدم بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل تحديث المستخدم",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpPost("SaveUserDevice")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveUserDeviceAsync(SaveDeviceDTO saveDeviceDTO)
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "").Trim();
                var user = await _accountService.GetUserFromToken(token);
                var result = await _accountService.SaveUserDeviceAsync(user.Id, saveDeviceDTO);

                if (result.Succeeded)
                {
                    return Ok(new BaseResponse
                    {
                        status = true,
                        Data = "تم حفظ الجهاز بنجاح"
                    });
                }

                return BadRequest(new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "فشل حفظ الجهاز",
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
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }

        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities()
        {
            try
            {
                var cities = await _accountService.GetCitiesAsync();
                return Ok(new BaseResponse
                {
                    status = true,
                    Data = cities
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse
                {
                    status = false,
                    ErrorCode = 500,
                    ErrorMessage = "حدث خطأ غير متوقع",
                    Data = ex.Message
                });
            }
        }
    }
}
