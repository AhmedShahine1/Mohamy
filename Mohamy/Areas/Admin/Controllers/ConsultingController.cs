//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Mohamy.BusinessLayer.Interfaces;

//namespace Mohamy.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Policy = "Admin")]
//    public class ConsultingController : Controller
//    {
//        private readonly IConsultingService _consultingService;
//        private readonly IAccountService _accountService;

//        public ConsultingController(IConsultingService consultingService, IAccountService accountService)
//        {
//            _consultingService = consultingService;
//            _accountService = accountService;
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
