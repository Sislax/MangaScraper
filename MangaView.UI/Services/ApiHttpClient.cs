using MangaScraper.Data.Models.Auth;
using MangaView.UI.Interfaces;
using MangaView.UI.Utiles;

namespace MangaView.UI.Services
{
    public class ApiHttpClient : IApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly Settings _settings;

        public ApiHttpClient(HttpClient httpClient, Settings settings)
        {
            _httpClient = httpClient;
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
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_settings.AuthServiceApiUrl + "Auth/Login", user);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginResponse>() ?? throw new Exception();
        }

        public async Task<LoginResponse> RefreshToken(string refreshToken)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(_settings.AuthServiceApiUrl + "Auth/RefreshToken", refreshToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<LoginResponse>() ?? throw new Exception();
        }
    }
}
