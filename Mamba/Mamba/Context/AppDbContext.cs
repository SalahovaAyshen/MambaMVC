using Mamba.Models;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Project> Projects { get; set; }

    }
}
