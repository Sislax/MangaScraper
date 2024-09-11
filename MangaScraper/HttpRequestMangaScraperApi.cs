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

        public async Task RequestOperate(HttpClient client)
        {
            string url = _settings.ApiUrl + _settings.EndPointOperate + "/" + _settings.NPagine;

            try
            {
                // Effettua la chiamata POST all'API
                HttpResponseMessage response = await client.PostAsync(url, null);

                // Verifica se la risposta ha avuto successo (status code 2xx)
                if (response.IsSuccessStatusCode)
                {
                    // Legge il contenuto della risposta
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Risposta API: {responseBody}", responseBody);
                }
                else
                {
                    _logger.LogError("Errore nella chiamata API: {response.StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore di comunicazione con l'API. Eccezione: {ex}", ex);
            }
        }

        public async Task RequestUpdate(HttpClient client)
        {
            try
            {
                // Effettua la chiamata POST all'API
                HttpResponseMessage response = await client.PostAsync(_settings.ApiUrl + _settings.EndPointUpdate, null);

                // Verifica se la risposta ha avuto successo (status code 2xx)
                if (response.IsSuccessStatusCode)
                {
                    // Legge il contenuto della risposta
                    var responseBody = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Risposta API: {responseBody}", responseBody);
                }
                else
                {
                    _logger.LogError("Errore nella chiamata API: {response.StatusCode}", response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore di comunicazione con l'API. Eccezione: {ex}", ex);
            }
        }
    }
}
