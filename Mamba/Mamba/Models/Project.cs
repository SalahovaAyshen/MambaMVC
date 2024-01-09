namespace Mamba.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Client { get; set; }
        public string ProjectDate { get; set; }
        public string ProjectUrl { get; set; }
        public string Detail { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
