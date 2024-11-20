
namespace MangaScraper.Data.Models.Auth
{
    public class LoginResponse
    {
        public string? JwtToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenExpired { get; set; }

        public LoginResponse()
        {
            
        }

        public LoginResponse(string token, string refreshToken, DateTime tokenExpired)
        {
            JwtToken = token;
            RefreshToken = refreshToken;
            TokenExpired = tokenExpired;
        }
    }
}
