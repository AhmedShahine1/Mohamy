//using AutoMapper;
//using Microsoft.AspNetCore.Mvc;
//using Mohamy.BusinessLayer.Interfaces;
//using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
//using Mohamy.Core.DTO;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Mohamy.BusinessLayer.Services;
//using Mohamy.Core.DTO.ConsultingViewModel;
//using Mohamy.Core.Entity.ConsultingData;
//using Mohamy.Core.DTO.AuthViewModel.UpdateModel;
//using Microsoft.AspNetCore.Identity;
//using Mohamy.Core.Entity.ApplicationData;

//namespace Mohamy.Areas.Laywer.Controllers
//{
//    [Area("Lawyer")]
//    public class LawyerController : Controller
//    {
//        private readonly IAccountService accountService;
//        private readonly IMapper mapper;
//        private readonly IConsultingService consultingService;
//        private readonly ISubConsultingService _subConsultingService;
//        private readonly IExperienceService _experienceService;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public LawyerController(IAccountService _accountService, IMapper _mapper, IConsultingService _consultingService, ISubConsultingService subConsultingService, IExperienceService experienceService, UserManager<ApplicationUser> userManager)
//        {
//            accountService = _accountService;
//            mapper = _mapper;
//            consultingService = _consultingService;
//            _subConsultingService = subConsultingService;
//            _experienceService = experienceService;
//            _userManager = userManager;
//        }
//        [HttpGet]
//        public async Task<IActionResult> Register()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterLawyer model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var result = await accountService.RegisterLaywer(model);
//                    if (result.Succeeded)
//                    {
//                        return RedirectToAction("Login", "Auth", new { area = "" });
//                    }
//                    return View(model);
//                }
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                var errorViewModel = new ErrorViewModel
//                {
//                    Message = "خطا في تسجيل البيانات",
//                    StackTrace = ex.Message
//                };
//                return View("~/Views/Shared/Error.cshtml", errorViewModel);
//            }
//        }

//        [HttpPost]
//        [Authorize(Policy = "Lawyer")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var result = await accountService.Suspend(id);
//            if (result.Succeeded)
//            {
//                return RedirectToAction("Index", "Home", new { area = "" });
//            }
//            return RedirectToAction("Index", "Admin");
//        }

//        [HttpGet]
//        [Authorize(Policy = "Lawyer")]
//        public async Task<IActionResult> Manage()
//        {
//            try
//            {
//                var user = await _userManager.GetUserAsync(User);

//                if (user == null || user.Id == null)
//                {
//                    // Handle error (e.g., token is missing or invalid)
//                    var errorViewModel = new ErrorViewModel
//                    {
//                        Message = "Invalid token.",
//                        StackTrace = "Token is either missing or invalid."
//                    };
//                    return View("~/Views/Shared/Error.cshtml", errorViewModel);
//                }

//                string LawyerId = user.Id;

//                // Fetch experiences and sub-consultings
//                var subConsultings = await _subConsultingService.GetAllAsync();
//                var experiences = await _experienceService.GetAllExperiencesByUserIdAsync(LawyerId);

//                // Create ExperienceDTO
//                var experienceDTO = new ExperienceDTO
//                {
//                    LaywerId = LawyerId,
//                    SubConsultings = subConsultings.Select(sc => new SelectListItem
//                    {
//                        Value = sc.Id,
//                        Text = sc.Name,
//                        Selected = experiences.Any(e => e.subConsultingId == sc.Id)
//                    }).ToList()
//                };

//                var personalUser = new PersonalUpdate
//                {
//                    FullName = user.FullName,
//                    PhoneNumber = user.PhoneNumber,
//                };
//                var QualificationUser = new QualificationUser
//                {
//                    //Description = user.Description,
//                    //YearsExperience = user.yearsExperience ?? 0,
//                    //City = user.City,
//                    //AcademicSpecialization = user.academicSpecialization,
//                    //CostConsulting = user.CostConsulting,
//                    //Education = user.Education,
//                };
//                // Combine into a new ViewModel
//                var model = new ManageLawyerViewModel
//                {
//                    Experience = experienceDTO,
//                    personalUpdate = personalUser,
//                    qualificationUser = QualificationUser,
//                };

//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                var errorViewModel = new ErrorViewModel
//                {
//                    Message = "خطا في تسجيل البيانات",
//                    StackTrace = ex.Message
//                };
//                return View("~/Views/Shared/Error.cshtml", errorViewModel);
//            }
//        }

//        [HttpPost]
//        [Authorize(Policy = "Lawyer")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Manage(ExperienceDTO model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var user = await _userManager.GetUserAsync(User);

//                    if (user == null || user.Id == null)
//                    {
//                        // Handle error (e.g., token is missing or invalid)
//                        var errorViewModel = new ErrorViewModel
//                        {
//                            Message = "Invalid token.",
//                            StackTrace = "Token is either missing or invalid."
//                        };
//                        return View("~/Views/Shared/Error.cshtml", errorViewModel);
//                    }
//                    model.LaywerId = user.Id;
//                    await _experienceService.ManageUserExperiencesAsync(model);
//                }
//                return RedirectToAction(nameof(Manage));
//            }
//            catch (Exception ex)
//            {
//                var errorViewModel = new ErrorViewModel
//                {
//                    Message = "خطا في تسجيل البيانات",
//                    StackTrace = ex.Message
//                };
//                return View("~/Views/Shared/Error.cshtml", errorViewModel);
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> PersonalUpdate(ManageLawyerViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.GetUserAsync(User);

//                if (user == null)
//                {
//                    // Handle error (e.g., token is missing or invalid)
//                    var errorViewModel = new ErrorViewModel
//                    {
//                        Message = "Invalid token.",
//                        StackTrace = "Token is either missing or invalid."
//                    };
//                    return View("~/Views/Shared/Error.cshtml", errorViewModel);
//                }

//                var result = await accountService.UpdatePersonalLaywer(user.Id, model.personalUpdate);
//                if (result.Succeeded)
//                {

//                    var cookieOptions = new CookieOptions
//                    {
//                        HttpOnly = true,
//                        Secure = true,
//                        Expires = DateTimeOffset.UtcNow.AddDays(1),
//                        SameSite = SameSiteMode.Strict
//                    };
//                    user = await _userManager.GetUserAsync(User);
//                    HttpContext.Response.Cookies.Append("userProfileImage", await accountService.GetUserProfileImage(user.ProfileId), cookieOptions);
//                    HttpContext.Response.Cookies.Append("userName", user.FullName, cookieOptions);

//                    return RedirectToAction(nameof(Manage));
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }
//            return RedirectToAction(nameof(Manage));
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> QualificationUpdate(ManageLawyerViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await _userManager.GetUserAsync(User);

//                if (user == null || user.Id == null)
//                {
//                    // Handle error (e.g., token is missing or invalid)
//                    var errorViewModel = new ErrorViewModel
//                    {
//                        Message = "Invalid token.",
//                        StackTrace = "Token is either missing or invalid."
//                    };
//                    return View("~/Views/Shared/Error.cshtml", errorViewModel);
//                }

//                var result = await accountService.UpdateQualificationLaywer(user.Id, model.qualificationUser);
//                if (result.Succeeded)
//                {
//                    return RedirectToAction(nameof(Manage));
//                }
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }
//            return RedirectToAction(nameof(Manage));
//        }

//        [HttpGet]
//        [Authorize(Policy = "Lawyer")]
//        public async Task<IActionResult> BankDetail()
//        {
//            try
//            {
//                var user = await _userManager.GetUserAsync(User);

//                if (user == null || user.Id == null)
//                {
//                    // Handle error (e.g., token is missing or invalid)
//                    var errorViewModel = new ErrorViewModel
//                    {
//                        Message = "Invalid token.",
//                        StackTrace = "Token is either missing or invalid."
//                    };
//                    return View("~/Views/Shared/Error.cshtml", errorViewModel);
//                }

//                string LawyerId = user.Id;
//                var model = new BankDetails
//                {
//                    //AccountNumber = user.AccountNumber,
//                    //IBAN = user.IBAN,
//                    //BeneficiaryName = user.BeneficiaryName,
//                    //BankName = user.BankName
//                };
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                var errorViewModel = new ErrorViewModel
//                {
//                    Message = "خطا في تسجيل البيانات",
//                    StackTrace = ex.Message
//                };
//                return View("~/Views/Shared/Error.cshtml", errorViewModel);
//            }
//        }

//        [HttpPost]
//        [Authorize(Policy = "Lawyer")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> BankDetail(BankDetails model)
//        {
//            try
//            {
//                if (ModelState.IsValid)
//                {
//                    var user = await _userManager.GetUserAsync(User);

//                    if (user == null || user.Id == null)
//                    {
//                        // Handle error (e.g., token is missing or invalid)
//                        var errorViewModel = new ErrorViewModel
//                        {
//                            Message = "Invalid token.",
//                            StackTrace = "Token is either missing or invalid."
//                        };
//                        return View("~/Views/Shared/Error.cshtml", errorViewModel);
//                    }
//                    await accountService.UpdateBankDetails(user.Id, model);
//                }
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                var errorViewModel = new ErrorViewModel
//                {
//                    Message = "خطا في تسجيل البيانات",
//                    StackTrace = ex.Message
//                };
//                return View("~/Views/Shared/Error.cshtml", errorViewModel);
//            }
//        }


//    }
//}
