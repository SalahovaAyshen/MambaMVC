using Mamba.Context;
using Mamba.Models;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Slider> slider = await _context.Sliders.ToListAsync();
            List<Project> projects = await _context.Projects.ToListAsync();
            HomeVM homeVM = new HomeVM
            {
                Sliders = slider,
                Projects = projects,
            };
            return View(homeVM);
        }
    }
}
