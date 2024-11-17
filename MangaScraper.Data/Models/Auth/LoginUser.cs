using System.ComponentModel.DataAnnotations;

namespace MangaScraper.Data.Models.Auth
{
    public class LoginUser
    {
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
