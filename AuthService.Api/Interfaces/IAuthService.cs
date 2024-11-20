using MangaScraper.Data.Models.Auth;

namespace AuthService.Api.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegistrationUser userData);
        Task<LoginResponse> LoginAsync(LoginUser user);
        //Task<LoginResponse> SetUserRefreshToken(RefreshTokenModel refreshTokenModel);
        Task<LoginResponse> RefreshTokenExist(string refreshToken);
    }
}
