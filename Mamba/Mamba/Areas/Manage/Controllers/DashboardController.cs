using Microsoft.AspNetCore.Mvc;

namespace Mamba.Areas.Manage.Controllers
{
    [Area("Manage")]
    
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
