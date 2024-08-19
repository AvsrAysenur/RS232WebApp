using Microsoft.EntityFrameworkCore;
using RS232WebApp.Models;

namespace RS232WebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User>? Users { get; set; }
    }
}
