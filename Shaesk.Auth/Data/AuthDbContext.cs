using Microsoft.EntityFrameworkCore;
using Shaesk.Auth.Entities;

namespace Shaesk.Auth.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
