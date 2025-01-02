using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;

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
    }
}
