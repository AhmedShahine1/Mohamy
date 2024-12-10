//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Mohamy.BusinessLayer.Interfaces;

//namespace Mohamy.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    [Authorize(Policy = "Admin")]
//    public class ManageController : Controller
//    {
//        private readonly IAdminService _adminService;
//        private readonly IConsultingService _consultingService;

//        public ManageController(IAdminService adminService, IConsultingService consultingService)
//        {
//            _adminService = adminService;
//            _consultingService = consultingService;
//        }

//        // Method to display all lawyers
//        public async Task<IActionResult> AllLawyers()
//        {
//            var lawyers = await _adminService.GetAllLawyersAsync();
//            return View(lawyers);
//        }

//        // Method to display all customers
//        public async Task<IActionResult> AllCustomers()
//        {
//            var customers = await _adminService.GetAllCustomersAsync();
//            return View(customers);
//        }

//        // Method to display all consultings assigned to lawyers
//        public async Task<IActionResult> AllConsultings()
//        {
//            var consultings = await _consultingService.GetAllConsultingsAsync();
//            return View(consultings);
//        }

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
