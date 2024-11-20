using MangaScraper.Data.Models.Auth;

namespace MangaView.UI.Interfaces
{
    public interface IAuthApiHttpClient
    {
        public Task<string> RegisterUserAsync(RegistrationUser user);

        public Task<LoginResponse> LoginUserAsync(LoginUser user);
    }
}
