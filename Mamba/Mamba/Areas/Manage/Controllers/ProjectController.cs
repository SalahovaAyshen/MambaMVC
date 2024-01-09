using Mamba.Areas.Manage.ViewModels;
using Mamba.Context;
using Mamba.Models;
using Mamba.Utilities.Extensions;
using Mamba.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProjectController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int count = await _context.Projects.CountAsync();
            List<Project> projects =await _context.Projects.Skip((page-1)*2).Take(2).ToListAsync();
            PaginationVM<Project> paginationVM = new PaginationVM<Project>
            {
                TotalPage = Math.Ceiling((double)count / 2),
                CurrentPage = page,
                Items = projects
            };
            return View(paginationVM);
        }

        public async Task<IActionResult> Create()
        {
            CreateProjectVM projectVM = new CreateProjectVM();
            projectVM.Categories=await _context.Categories.ToListAsync();
            return View(projectVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectVM projectVM)
        {
            projectVM.Categories = await _context.Categories.ToListAsync();
            if(!ModelState.IsValid) return View(projectVM);
            if(!await _context.Categories.AnyAsync(c => c.Id == projectVM.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Not found category id");
                return View(projectVM);
            }
            if (!projectVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "Wrong type");
                return View(projectVM);
            }
            if (!projectVM.Photo.ValidateSize(2*1024))
            {
                ModelState.AddModelError("Photo", "Wrong size");
                return View(projectVM);
            }
            string filename = await projectVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "portfolio");
            Project project = new Project
            {
                Name = projectVM.Name,
                Client = projectVM.Client,
                Detail = projectVM.Detail,
                ProjectDate = projectVM.ProjectDate,
                ProjectUrl = projectVM.ProjectUrl,
                CategoryId = projectVM.CategoryId,
                ImageUrl = filename,
            };
            await _context.AddAsync(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        
        }

    }
}
