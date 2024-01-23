using Mamba.Models;

namespace Mamba.ViewModels
{
    public class HomeVM
    {
        public ICollection<Slider> Sliders { get; set; }
        public ICollection<Project> Projects { get; set; }
        public Dictionary<string, string> Settings { get; set; }
    }
}
