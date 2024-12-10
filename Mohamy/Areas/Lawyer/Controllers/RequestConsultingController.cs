//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Mohamy.BusinessLayer.Interfaces;
//using Mohamy.BusinessLayer.Services;
//using Mohamy.Core.DTO.ConsultingViewModel;
//using Mohamy.Core.Entity.ApplicationData;
//using Mohamy.Core.Helpers;

//namespace Mohamy.Areas.Laywer.Controllers
//{
//    [Area("Lawyer")]
//    [Authorize(Policy = "Lawyer")]
//    public class RequestConsultingController : Controller
//    {
//        private readonly IRequestConsultingService _requestConsultingService;
//        private readonly IAccountService _accountService;
//        private readonly IConsultingService _consultingService;
//        private readonly UserManager<ApplicationUser> _userManager;
//        private ApplicationUser? CurrentUser;

//        public RequestConsultingController(IRequestConsultingService requestConsultingService, IAccountService accountService, IConsultingService consultingService, UserManager<ApplicationUser> userManager)
//        {
//            _requestConsultingService = requestConsultingService;
//            _accountService = accountService;
//            _consultingService = consultingService;
//            _userManager = userManager;
//        }

//        // GET: RequestConsulting
//        [HttpGet]
//        public async Task<IActionResult> Index()
//        {
//            var userId = _userManager.GetUserId(User);
//            CurrentUser = _accountService.GetUserById(userId).Result;
//            var requests = await _requestConsultingService.GetRequestsByUserAsync(CurrentUser.Id);
//            return View(requests);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> RequestConsulting(string consultingId)
//        {
//            var userId = _userManager.GetUserId(User);
//            CurrentUser = _accountService.GetUserById(userId).Result;
//            if (ModelState.IsValid)
//            {
//                var consulting = await _consultingService.GetConsultingByIdAsync(consultingId);
//                var requestConsultingDTO = new RequestConsultingDTO()
//                {
//                    LawyerId = CurrentUser.Id,
//                    ConsultingId = consultingId,
//                };
//                var addedRequest = await _requestConsultingService.AddRequestAsync(requestConsultingDTO);
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        // POST: RequestConsulting/UpdateStatus
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> CancelRequest(string requestId)
//        {
//            var userId = _userManager.GetUserId(User);
//            CurrentUser = _accountService.GetUserById(userId).Result;
//            if (string.IsNullOrEmpty(requestId))
//            {
//                return RedirectToAction(nameof(Index));
//            }

//            var success = await _requestConsultingService.UpdateRequestStatusAsync(requestId, statusRequestConsulting.Cancel);
//            if (success)
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            else
//            {
//                return RedirectToAction(nameof(Index));
//            }
//        }
//    }
//}
