using Mamba.Context;
using Mamba.Models;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;

        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id <= 0) return BadRequest();
            Project project = await _context.Projects
                .Include(p=>p.Category)
                .FirstOrDefaultAsync(p=>p.Id==id);
            if(project is null) return NotFound();

            ProjectVM projectVM = new ProjectVM
            {
                Project = project,
            };
            return View(projectVM);
        }
    }
}
