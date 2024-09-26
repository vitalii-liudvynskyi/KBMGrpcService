using KBMGrpcService.Repositories.Context.Models;
using Microsoft.EntityFrameworkCore;

namespace KBMGrpcService.Repositories.Context
{
    public class KBMDbContext : DbContext
    {
        public KBMDbContext(DbContextOptions<KBMDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Organization> Organizations { get; set; }

        public DbSet<User> Users { get; set; }
    }
}
