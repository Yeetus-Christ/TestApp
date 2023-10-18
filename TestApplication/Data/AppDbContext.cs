using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        public AppDbContext()
        {
                
        }

        public virtual DbSet<Dog> Dogs { get; set; }
    }
}
