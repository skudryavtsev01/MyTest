using Microsoft.EntityFrameworkCore;
using MyTest.Models;

namespace MyTest.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Category {  get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ApplicationType> ApplicationType { get; set; }
    }
}
