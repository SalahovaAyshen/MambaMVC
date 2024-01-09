namespace Mamba.Models
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Offer { get; set; } = null!;
        public int Order { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
