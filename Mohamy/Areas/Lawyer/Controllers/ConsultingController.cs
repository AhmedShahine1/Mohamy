//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Mohamy.BusinessLayer.Interfaces;
//using Mohamy.Core.Entity.ApplicationData;
//using Mohamy.Core.Helpers;

//namespace Mohamy.Areas.Lawyer.Controllers
//{
//    [Area("Lawyer")]
//    [Authorize(Policy = "Lawyer")]
//    public class ConsultingController : Controller
//    {
//        private readonly IConsultingService _consultingService;
//        private readonly IAccountService _accountService;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public ConsultingController(IConsultingService consultingService, IAccountService accountService, UserManager<ApplicationUser> userManager)
//        {
//            _consultingService = consultingService;
//            _accountService = accountService;
//            _userManager = userManager;
//        }

//        // Action to get all consultings without a lawyer
//        public async Task<IActionResult> ConsultingsWithoutLawyer()
//        {
//            var consultings = await _consultingService.GetConsultingsWithoutLawyerAsync();
//            return View(consultings);
//        }

//        // Action to get all consultings assigned to the logged-in lawyer
//        public async Task<IActionResult> MyConsultings()
//        {
//            var user = await _userManager.GetUserAsync(User);
//            var consultings = await _consultingService.GetConsultingsByLawyerIdAsync(user.Id, statusConsulting.InProgress);
//            return View(consultings);
//        }

//        // Action to get all consultings
//        public async Task<IActionResult> AllConsultings()
//        {
//            var consultings = await _consultingService.GetAllConsultingsAsync();
//            return View(consultings);
//        }

//        // Action to get details of a specific consulting
//        public async Task<IActionResult> ConsultingDetails(string id)
//        {
//            var consulting = await _consultingService.GetConsultingByIdAsync(id);
//            if (consulting == null)
//            {
//                return NotFound();
//            }
//            return View(consulting);
//        }
//    }
//}
