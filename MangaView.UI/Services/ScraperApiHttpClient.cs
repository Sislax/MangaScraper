using MangaScraper.Data.Models.Domain;
using MangaScraper.Data.Models.DTOs;
using MangaView.UI.Interfaces;
using MangaView.UI.Utiles;
using System.Text.Json;

namespace MangaView.UI.Services
{
    public class ScraperApiHttpClient : IScraperApiHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly Settings _settings;

        public ScraperApiHttpClient(HttpClient httpClient, Settings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<List<MangaDTO>> GetMangaDTOs()
        {
            try
            {
                List<MangaDTO>? result = await _httpClient.GetFromJsonAsync<List<MangaDTO>>(_settings.MangaViewApiUrl + "Manga/GetMangaDTOs");

                if (result == null)
                {
                    throw new InvalidOperationException("La risposta è stata ottenuta con successo ma nessun dato è stato ottenuto");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Errore durante la chiamata API: {ex.Message}", ex);
            }
            catch(JsonException ex)
            {
                throw new Exception("Errore durante il parsing della risposta API: {ex.Message}", ex);
            }
        }

        public async Task<MangaDTO> GetMangaDTOWithAllData(int mangaId)
        {
            try
            {
                MangaDTO? result = await _httpClient.GetFromJsonAsync<MangaDTO>(_settings.MangaViewApiUrl + $"Manga/GetMangaDTOWithAllData?id={mangaId}");

                if (result == null)
                {
                    throw new InvalidOperationException("La risposta è stata ottenuta con successo ma nessun dato è stato ottenuto");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Errore durante la chiamata API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Errore durante il parsing della risposta API: {ex.Message}", ex);
            }
        }

        public async Task<CapitoloDTO> GetCapitoloDTOWithData(int capitoloId)
        {
            try
            {
                CapitoloDTO? result = await _httpClient.GetFromJsonAsync<CapitoloDTO>(_settings.MangaScraperApiUrl + $"Manga/GetCapitoloDTOWithData?id={capitoloId}");

                if (result == null)
                {
                    throw new InvalidOperationException("La risposta è stata ottenuta con successo ma nessun dato è stato ottenuto");
                }

                return result;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Errore durante la chiamata API: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Errore durante il parsing della risposta API: {ex.Message}", ex);
            }
        }

        public string GetCurrentImage(int imageId)
        {
            return _settings.MangaScraperApiUrl + $"Manga/GetImageFromImageId?id={imageId}";
        }

        public string GetCopertina(int id)
        {
            return _settings.MangaViewApiUrl + $"Manga/GetCopertina?id={id}";
        }
    }
}
