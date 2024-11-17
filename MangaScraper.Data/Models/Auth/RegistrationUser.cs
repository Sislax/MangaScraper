using System.ComponentModel.DataAnnotations;

namespace MangaScraper.Data.Models.Auth
{
    public class RegistrationUser
    {
        public required string UserName { get; set; }

        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
