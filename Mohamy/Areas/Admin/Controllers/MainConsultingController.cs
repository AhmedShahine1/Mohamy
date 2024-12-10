using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.ConsultingViewModel;

namespace Mohamy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class MainConsultingController : Controller
    {
        private readonly IMainConsultingService _mainConsultingService;

        public MainConsultingController(IMainConsultingService mainConsultingService)
        {
            _mainConsultingService = mainConsultingService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewData["Title"] = "Main Consulting";
                var mainConsultings = await _mainConsultingService.GetAllAsync();
                return View(mainConsultings);
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "خطأ في جلب البيانات",
                    StackTrace = ex.Message
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["Title"] = "Main Consulting";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MainConsultingDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mainConsultingService.AddAsync(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var errorViewModel = new ErrorViewModel
                    {
                        Message = "خطأ في تسجيل البيانات",
                        StackTrace = ex.Message
                    };
                    return View("~/Views/Shared/Error.cshtml", errorViewModel);
                }
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                ViewData["Title"] = "Main Consulting";
                var dto = await _mainConsultingService.GetByIdAsync(id);
                return View(dto);
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "خطأ في جلب البيانات",
                    StackTrace = ex.Message
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MainConsultingDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _mainConsultingService.UpdateAsync(dto);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    var errorViewModel = new ErrorViewModel
                    {
                        Message = "خطأ في تعديل البيانات",
                        StackTrace = ex.Message
                    };
                    return View("~/Views/Shared/Error.cshtml", errorViewModel);
                }
            }
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _mainConsultingService.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    Message = "خطأ في حذف البيانات",
                    StackTrace = ex.Message
                };
                return View("~/Views/Shared/Error.cshtml", errorViewModel);
            }
        }
    }
}
