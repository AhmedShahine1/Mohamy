using Microsoft.AspNetCore.Mvc;
using Mohamy.Core.Helpers;
using Mohamy.Core;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;

namespace Mohamy.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IRequestConsultingService _requestConsultingService;
        private readonly IConsultingService _consultingService;

        public HomeController(IAdminService adminService, IRequestConsultingService requestConsultingService, IConsultingService consultingService)
        {
            _adminService = adminService;
            _requestConsultingService = requestConsultingService;
            _consultingService = consultingService;
        }



        public async Task<IActionResult> Index()
        {
            var lawyers = await _adminService.GetAllLawyersAsync();
            var customers = await _adminService.GetAllCustomersAsync();
            var admins = await _adminService.GetAllAdminsAsync();
            var requests = await _requestConsultingService.GetAllRequestsAsync();
            var completedConsultings = await _consultingService.GetConsultingsbyStatus(statusConsulting.Completed);
            var inProgressConsultings = await _consultingService.GetConsultingsbyStatus(statusConsulting.InProgress);
            var cancelledConsultings = await _consultingService.GetConsultingsbyStatus(statusConsulting.Cancelled);

            var model = new DashboardViewModel
            {
                TotalLawyers = lawyers.Count(),
                TotalCustomers = customers.Count(),
                TotalAdmins = admins.Count(),
                TotalRequests = requests.Count(),
                CompletedConsultings = completedConsultings.Count(),
                InProgressConsultings = inProgressConsultings.Count(),
                CancelledConsultings = cancelledConsultings.Count()
            };

            return View(model);
        }
    }
}
