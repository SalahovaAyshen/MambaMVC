using Mamba.Areas.Manage.ViewModels;
using Mamba.Context;
using Mamba.Models;
using Mamba.Utilities.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Areas.Manage.Controllers
{
    [Area("Manage")]

    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            ICollection<Slider> slider = await _context.Sliders.ToListAsync();
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSliderVM sliderVM)
        {
            if (!ModelState.IsValid) return View();
            if(await _context.Sliders.AnyAsync(s => s.Order == sliderVM.Order))
            {
                ModelState.AddModelError("Order", "The order already existed");
                return View();
            }
            if (!sliderVM.Photo.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "The image type must be IMAGE/");
                return View();
            }
            if (!sliderVM.Photo.ValidateSize(2 * 1024))
            {
                ModelState.AddModelError("Photo", "The image size can't be more than 4mb");
                return View();
            }
            string filename = await sliderVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slide");
            Slider slider = new Slider
            {
                Title = sliderVM.Title,
                Offer = sliderVM.Offer,
                Order = sliderVM.Order,
                ImageUrl = filename,
            };
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            UpdateSliderVM sliderVM = new UpdateSliderVM
            {
                Title = slider.Title,
                Offer = slider.Offer,
                Order = slider.Order,
            };
            return View(sliderVM);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSliderVM sliderVM)
        {
            if (!ModelState.IsValid) return View();
            if (id <= 0) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            if( sliderVM.Order<0)
            {
                ModelState.AddModelError("Order", "The order can't be a negative number ");
                return View();
            }
            if(sliderVM.Photo is not null)
            {
                if (!sliderVM.Photo.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "The image type must be IMAGE/");
                    return View();
                }
                if (!sliderVM.Photo.ValidateSize(2 * 1024))
                {
                    ModelState.AddModelError("Photo", "The image size can't be more than 4mb");
                    return View();
                }
                string filename = await sliderVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "img", "slide");
                slider.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img", "slide");
                slider.ImageUrl = filename;
            }
            slider.Title = sliderVM.Title;
            slider.Offer = sliderVM.Offer;
            slider.Order = sliderVM.Order;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Slider slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            slider.ImageUrl.DeleteFile(_env.WebRootPath, "assets", "img", "slide");
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
