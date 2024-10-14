using MangaView.Interfaces;
using MangaView.Models;

namespace MangaView.Services
{
	public class MangaService : IMangaService
	{
		private readonly HttpClient _httpClient;
		private readonly Settings _settings;

        public MangaService(HttpClient httpClient, Settings settings)
        {
            _httpClient = httpClient;
			_settings = settings;
        }
        public async Task<List<MangaDTO>> GetAllMangas()
		{
			return await _httpClient.GetFromJsonAsync<List<MangaDTO>>(_settings.ApiUrl + "Manga/GetMangaDTOs") ?? throw new Exception();
		}
	}
}
