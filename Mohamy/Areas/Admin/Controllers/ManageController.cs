using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Mohamy.Core.Entity.ApplicationData;

namespace Mohamy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class ManageController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IConsultingService _consultingService;
        private readonly IAccountService _accountService;

        public ManageController(IAdminService adminService, IConsultingService consultingService, IAccountService accountService)
        {
            _adminService = adminService;
            _consultingService = consultingService;
            _accountService = accountService;
        }

        // Display all lawyers
        public async Task<IActionResult> AllLawyers()
        {
            var lawyers = await _adminService.GetAllLawyersAsync();
            return View(lawyers);
        }

        public async Task<IActionResult> LawyerDetails(string lawyerId)
        {
            var lawyer = await _adminService.GetLawyerByIdAsync(lawyerId);
            if (lawyer == null)
            {
                return NotFound();
            }
            return View(lawyer);
        }

        // Display all customers
        public async Task<IActionResult> AllCustomers()
        {
            var customers = await _adminService.GetAllCustomersAsync();
            return View(customers);
        }

        // Display all consultings assigned to lawyers
        public async Task<IActionResult> AllConsultings()
        {
            var consultings = await _consultingService.GetAllConsultingsAsync();
            return View(consultings);
        }

        // Get details of a specific consulting request
        public async Task<IActionResult> ConsultingDetails(string id)
        {
            var consulting = await _consultingService.GetConsultingByIdAsync(id);
            if (consulting == null)
            {
                return NotFound();
            }
            return View(consulting);
        }
        // Activate a lawyer's account
        [HttpPost]
        public async Task<IActionResult> Activate(string userId)
        {
            var result = await _accountService.Activate(userId);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تفعيل حساب المحامي بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل في تفعيل حساب المحامي.";
            }
            return ReturnToPreviousView();
        }

        // Suspend a lawyer's account
        [HttpPost]
        public async Task<IActionResult> Suspend(string userId)
        {
            var result = await _accountService.Suspend(userId);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم تعطيل حساب المحامي بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل في تعطيل حساب المحامي.";
            }
            return ReturnToPreviousView();
        }

        // Change lawyer's registration status
        [HttpPost]
        public async Task<IActionResult> ChangeLawyerRegistrationStatus(string lawyerId)
        {
            var result = await _accountService.ChangeLawyerRegistrationStatus(lawyerId);

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "تم تحديث حالة تسجيل المحامي بنجاح.";
            }
            else
            {
                TempData["ErrorMessage"] = $"فشل في تحديث الحالة: {result.Message}";
            }
            return ReturnToPreviousView();
        }

        // Delete a lawyer's account
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            var result = await _accountService.DeleteAccountAsync(userId);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "تم حذف الحساب بنجاح!";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل في حذف الحساب!";
            }

            return ReturnToPreviousView();
        }

        // Helper method to return to the previous view
        private IActionResult ReturnToPreviousView()
        {
            string referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }
            return RedirectToAction("Index"); // Default view if referer is not available
        }
    }
}
