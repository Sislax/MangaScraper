using Microsoft.Extensions.Logging;

namespace MangaScraper
{
    public class HttpRequestMangaScraperApi
    {
        private readonly Settings _settings;
        private readonly ILogger<HttpRequestMangaScraperApi> _logger;

        public HttpRequestMangaScraperApi(Settings settings, ILogger<HttpRequestMangaScraperApi> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task RequestUpdate(HttpClient client)
        {
            string url = _settings.ApiUrl + _settings.EndPointUpdate + "/" + _settings.NPagine;

            try
            {
                // Effettua la chiamata POST all'API
                HttpResponseMessage response = await client.PostAsync(url, null);

                // Verifica se la risposta ha avuto successo (status code 2xx)
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Status Code risposta API: {response.StatusCode.ToString()}", response.StatusCode.ToString());
                }
                else
                {
                    _logger.LogWarning("Errore nella chiamata API: {response.StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore di comunicazione con l'API. Eccezione: {ex}", ex);
            }
        }
    }
}
