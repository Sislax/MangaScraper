using MangaScraper.Data.Models.Auth;
using MangaView.UI.Interfaces;
using MangaView.UI.Utiles;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Security.Cryptography;

namespace MangaView.UI.Services
{
    public class LoginService
    {
        private readonly ProtectedLocalStorage _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly IApiHttpClient _apiHttpClient;
        private readonly Settings _settings;

        public LoginService(ProtectedLocalStorage localStorage, NavigationManager navigationManager, IApiHttpClient apiHttpClient, Settings settings)
        {
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _apiHttpClient = apiHttpClient;
            _settings = settings;
        }

        public async Task<bool> LoginAsync(LoginUser user)
        {
            LoginResponse response = await _apiHttpClient.LoginUserAsync(user);

            if (string.IsNullOrEmpty(response?.JwtToken))
            {
                return false;
            }

            await _localStorage.SetAsync("JwtToken", response.JwtToken);
            await _localStorage.SetAsync("RefreshToken", response.RefreshToken!);

            return true;
        }

        public async Task<List<Claim>> GetLoginInfoAsync()
        {
            List<Claim> result = new List<Claim>();

            ProtectedBrowserStorageResult<string> jwtToken;
            ProtectedBrowserStorageResult<string> refreshToken;

            try
            {
                jwtToken = await _localStorage.GetAsync<string>("JwtToken");
                refreshToken = await _localStorage.GetAsync<string>("RefreshToken");
            }
            catch (CryptographicException ex)
            {
                await LogoutAsync();
                return result;
            }

            if(!jwtToken.Success || jwtToken.Value == default)
            {
                return result;
            }

            var claims = JwtTokenHelper.ValidateDecodeToken(jwtToken.Value, _settings);

            if(claims.Count != 0)
            {
                return claims;
            }

            if (refreshToken.Value != default)
            {
                LoginResponse response = await _apiHttpClient.RefreshToken(refreshToken.Value);

                if (!string.IsNullOrWhiteSpace(response?.JwtToken))
                {
                    await _localStorage.SetAsync("JwtToken", response.JwtToken);
                    await _localStorage.SetAsync("RefreshToken", response.RefreshToken!);

                    claims = JwtTokenHelper.ValidateDecodeToken(response.JwtToken, _settings);
                }
                else
                {
                    await LogoutAsync();
                }
            }
            else
            {
                await LogoutAsync();
            }

            return claims;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.DeleteAsync("JwtToken");
            await _localStorage.DeleteAsync("RefreshToken");

            _navigationManager.NavigateTo("/", true);
        }
    }
}
