namespace Mamba.Areas.Manage.ViewModels
{
    public class CreateSliderVM
    {
        public string Title { get; set; }
        public string Offer { get; set; }
        public int Order { get; set; }
        public IFormFile Photo { get; set; } = null!;
    }
}
