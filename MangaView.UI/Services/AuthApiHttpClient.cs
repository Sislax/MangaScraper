using MangaScraper.Data.Models.Auth;
using MangaView.UI.Interfaces;
using MangaView.UI.Utiles;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;

namespace MangaView.UI.Services
{
    public class AuthApiHttpClient : IAuthApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ProtectedLocalStorage _localStorage;
        private readonly CustomAuthStateProvider _customAuthStateProvider;
        private readonly Settings _settings;

        public AuthApiHttpClient(HttpClient httpClient, ProtectedLocalStorage localStorage, CustomAuthStateProvider customAuthStateProvider , Settings settings)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _customAuthStateProvider = customAuthStateProvider;
            _settings = settings;
        }

        public async Task<string> RegisterUserAsync(RegistrationUser user)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_settings.AuthServiceApiUrl + "Auth/Register", user);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<LoginResponse> LoginUserAsync(LoginUser user)
        {
            await SetAuthorizeHeader();

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_settings.AuthServiceApiUrl + "Auth/Login", user);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginResponse>() ?? throw new Exception();
        }

        private async Task SetAuthorizeHeader()
        {
            LoginResponse sessionState = (await _localStorage.GetAsync<LoginResponse>("SessionState")).Value!;

            if (sessionState != null && !string.IsNullOrEmpty(sessionState.JwtToken))
            {
                if(sessionState.TokenExpired < DateTime.Now)
                {
                    await _customAuthStateProvider.NotifyUserLogout();
                }
                else if(sessionState.TokenExpired < DateTime.Now.AddMinutes(10))
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_settings.AuthServiceApiUrl + "Auth/RefreshTokenAndLogin", sessionState.RefreshToken);

                    LoginResponse content = await response.Content.ReadFromJsonAsync<LoginResponse>() ?? throw new Exception();

                    if(content != null)
                    {
                        await _customAuthStateProvider.NotifyUserLogin(content);

                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", content.JwtToken);
                    }
                    else
                    {
                        await _customAuthStateProvider.NotifyUserLogout();
                    }
                }
                else
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionState.JwtToken);
                }
            }
        }
    }
}
