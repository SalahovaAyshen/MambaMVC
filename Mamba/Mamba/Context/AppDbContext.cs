using Mamba.Models;
using Microsoft.EntityFrameworkCore;

namespace Mamba.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Setting> Settings { get; set; }      
    }
}
