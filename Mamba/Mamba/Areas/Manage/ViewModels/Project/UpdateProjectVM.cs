using Mamba.Models;

namespace Mamba.Areas.Manage.ViewModels
{
    public class UpdateProjectVM
    {
        public string Name { get; set; }
        public string Client { get; set; }
        public string Detail { get; set; }
        public string ProjectDate { get; set; }
        public string ProjectUrl { get; set; }
        public IFormFile? Photo { get; set; } = null!;
        public int CategoryId { get; set; }
        public ICollection<Category>? Categories { get; set; }
    }
}
