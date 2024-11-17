using Microsoft.AspNetCore.Identity;

namespace MangaScraper.Data.Models.Auth
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        public User(string name, string email) : base(name)
        {
            base.Email = email;
        }
    }
}
