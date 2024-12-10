using Microsoft.AspNetCore.Mvc;

namespace Mohamy.Controllers.MVC
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
