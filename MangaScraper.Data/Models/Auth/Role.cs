using Microsoft.AspNetCore.Identity;

namespace MangaScraper.Data.Models.Auth
{
    public class Role : IdentityRole
    {
        public Role(string name) : base(name)
        {
            
        }
    }
}
