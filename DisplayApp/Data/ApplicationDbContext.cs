using DisplayApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace DisplayApp.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Comments> Comments { get; set; }
    }
}
