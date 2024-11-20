using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MangaScraper.Data.Data
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
        //public readonly DbContextOptions<UserIdentityDbContext> _options;
        public UserIdentityDbContext(DbContextOptions<UserIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
