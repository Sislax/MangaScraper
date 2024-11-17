using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MangaScraper.Data.Data
{
    public class UserIdentityDbContext : IdentityDbContext<User>
    {
    }
}
