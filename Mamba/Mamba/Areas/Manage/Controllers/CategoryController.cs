using Mamba.Areas.Manage.ViewModels;
using Mamba.Context;
using Mamba.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page =1)
        {
            int count = await _context.Categories.CountAsync();
            List<Category> categories = await _context.Categories.Skip((page-1)*2).Take(2).Include(c=>c.Projects).ToListAsync();
            PaginationVM<Category> pagination = new PaginationVM<Category>
            {
                TotalPage = Math.Ceiling((double)count / 2),
                CurrentPage = page,
                Items = categories
            };
            return View(pagination);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryVM categoryVM)
        {
            if (!ModelState.IsValid) return View();
            if(await _context.Categories.AnyAsync(e => e.Name == categoryVM.Name))
            {
                ModelState.AddModelError("Name", "The category name already existed");
                return View();
            }
            Category category = new Category{ Name = categoryVM.Name };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            UpdateCategoryVM categoryVM = new UpdateCategoryVM
            { Name = category.Name };
            return View(categoryVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateCategoryVM categoryVM)
        {
            if(id<=0) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();

            if (!ModelState.IsValid) return View(categoryVM);
            if(await _context.Categories.AnyAsync(c=>c.Name== categoryVM.Name))
            {
                ModelState.AddModelError("Name", "The category name already existed");
                return View(categoryVM);
            }
            category.Name = categoryVM.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id<=0) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c=>c.Id == id);
            if (category == null) return NotFound();
            Project project = await _context.Projects.FirstOrDefaultAsync(p => p.CategoryId == id);
            if(project is not null)
            {
                project.CategoryId = null;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
