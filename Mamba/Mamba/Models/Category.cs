namespace Mamba.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Project> Projects { get; set; }
    }
}
