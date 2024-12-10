using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.DTO;
using Mohamy.Core.DTO.ConsultingViewModel;

namespace Mohamy.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "Admin")]
    public class SubConsultingController : Controller
    {
        private readonly ISubConsultingService _subConsultingService;
        private readonly IMainConsultingService _mainConsultingService;

        public SubConsultingController(ISubConsultingService subConsultingService, IMainConsultingService mainConsultingService)
        {
            _subConsultingService = subConsultingService;
            _mainConsultingService = mainConsultingService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewData["Title"] = "Sub-Consulting";
                var subConsultings = await _subConsultingService.GetAllAsync();
                return View(subConsultings);
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
        public async Task<IActionResult> Create()
        {
            ViewData["Title"] = "Sub-Consulting";
            ViewBag.MainConsultings = await _mainConsultingService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubConsultingDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _subConsultingService.AddAsync(dto);
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
            ViewBag.MainConsultings = await _mainConsultingService.GetAllAsync();
            return View(dto);
        }

        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var dto = await _subConsultingService.GetByIdAsync(id);
                ViewData["Title"] = $"Sub-Consulting {dto.Name}";
                ViewBag.MainConsultings = await _mainConsultingService.GetAllAsync();
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
        public async Task<IActionResult> Edit(SubConsultingDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _subConsultingService.UpdateAsync(dto);
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
            ViewBag.MainConsultings = await _mainConsultingService.GetAllAsync();
            return View(dto);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _subConsultingService.DeleteAsync(id);
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
