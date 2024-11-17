using MangaScraper.Data.Models.Auth;

namespace MangaView.UI.Interfaces
{
    public interface IApiHttpClient
    {
        public Task<string> RegisterUserAsync(RegistrationUser user);

        public Task<LoginResponse> LoginUserAsync(LoginUser user);

        public Task<LoginResponse> RefreshToken(string refreshToken);
    }
}
