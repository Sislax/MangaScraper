
namespace MangaScraper.Data.Models.Auth
{
    public class LoginResponse
    {
        public bool IsLoggedIn { get; set; } = false;
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }

        public LoginResponse()
        {
            
        }

        public LoginResponse(string token, string refreshToken)
        {
            JwtToken = token;
            RefreshToken = refreshToken;
        }
    }
}
