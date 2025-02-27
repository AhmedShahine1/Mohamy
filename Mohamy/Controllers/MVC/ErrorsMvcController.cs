using Microsoft.AspNetCore.Mvc;

namespace Mohamy.Controllers.MVC
{
    public class ErrorsMvcController : Controller
    {
        public IActionResult Index(int code, string? message = null)
        {
            ViewBag.ErrorCode = code;
            ViewBag.ErrorMessage = message ?? "An unexpected error occurred.";
            return View("Error");
        }
    }
}
