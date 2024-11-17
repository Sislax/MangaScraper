
namespace MangaScraper.Data.Models.Auth
{
    public class RefreshTokenModel
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }

        public RefreshTokenModel(string jwtToken, string refreshToken)
        {
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
