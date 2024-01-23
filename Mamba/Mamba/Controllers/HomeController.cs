using Mamba.Context;
using Mamba.Models;
using Mamba.Services;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly LayoutService _service;

        public HomeController(AppDbContext context, LayoutService service)
        {
            _context = context;
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _context.Sliders.ToListAsync();
            List<Project> projects = await _context.Projects.ToListAsync();
            Dictionary<string, string> settings = await _service.GetSettingsAsync();

            HomeVM homeVM = new HomeVM
            {
                Sliders = slider,
                Projects = projects,
                Settings = settings
            };
            return View(homeVM);
        }
    }
}
