using AutoMapper;
using Mohamy.BusinessLayer.AutoMapper;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.AuthViewModel.RegisterModel;
using Mohamy.Core.Entity.ApplicationData;
using Mohamy.Core.Entity.Files;
using Mohamy.RepositoryLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mohamy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountService accountService;
        public AdminController(IAccountService _accountService)
        {
            accountService = _accountService;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterAdmin model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await accountService.RegisterAdmin(model);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Auth", new { area = "" });
                    }
                    return View(model);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "خطا في تسجيل البيانات",
                    StackTrace = ex.InnerException.Message
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var admin = await accountService.GetUserById(id);
            if (admin == null)
            {
                return NotFound();
            }
            var model = new RegisterAdmin
            {
                FullName = admin.FullName,
                PhoneNumber = admin.PhoneNumber
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, RegisterAdmin model)
        {
            if (ModelState.IsValid)
            {
                var result = await accountService.UpdateAdmin(id, model);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await accountService.Suspend(id);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            return RedirectToAction("Index", "Admin");
        }
    }
}
