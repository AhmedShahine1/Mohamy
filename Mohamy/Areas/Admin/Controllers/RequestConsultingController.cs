using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.Helpers;

namespace Mohamy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class RequestConsultingController : Controller
    {
        private readonly IRequestConsultingService _requestConsultingService;
        public RequestConsultingController(IRequestConsultingService requestConsultingService)
        {
            _requestConsultingService = requestConsultingService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _requestConsultingService.GetAllRequestsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CancelRequest(string requestId)
        {
            await _requestConsultingService.UpdateRequestStatusAsync(requestId, statusRequestConsulting.Cancel);
            return RedirectToAction(nameof(Index));
        }

    }
}
