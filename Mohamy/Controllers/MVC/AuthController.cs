using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO.AuthViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.Controllers.MVC
{
    public class AuthController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewData["Title"] = "Access Denied";
            return View();
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            ViewData["Title"] = "Login";
            ViewData["ReturnUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string ReturnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var (isSuccess, token, errorMessage) = await _accountService.Login(model);

            if (isSuccess)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(30) : DateTimeOffset.UtcNow.AddHours(1)
                };
                var user =await _userManager.GetUserAsync(User);
                HttpContext.Response.Cookies.Append("userProfileImage", await _accountService.GetUserProfileImage(user.ProfileId), cookieOptions);
                HttpContext.Response.Cookies.Append("userName", user.FullName, cookieOptions);

                if (Url.IsLocalUrl(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, errorMessage);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var isLoggedOut = await _accountService.Logout(user);
                if (isLoggedOut)
                {
                    HttpContext.Response.Cookies.Delete("userProfileImage");
                    HttpContext.Response.Cookies.Delete("userName");
                    return RedirectToAction("Login");
                }
            }

            return View();
        }
    }
}
