using Delicious.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Delicious.DataContext
{
    public class DeliciousDbContext:IdentityDbContext<AppUser>
    {
        public DeliciousDbContext(DbContextOptions<DeliciousDbContext> options): base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }

        public DbSet<Work> Works { get; set; }
    }
}
