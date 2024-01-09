using Microsoft.AspNetCore.Mvc;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
