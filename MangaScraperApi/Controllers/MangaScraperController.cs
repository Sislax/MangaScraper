using Hangfire;
using MangaScraperApi.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaScraper.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MangaScraperController : ControllerBase
    {
        public readonly IMangaScraperService _mangaScraperService;
        public readonly ILogger<MangaScraperController> _logger;
        public MangaScraperController(IMangaScraperService mangaScraperService, ILogger<MangaScraperController> logger)
        {
            _mangaScraperService = mangaScraperService;
            _logger = logger;
        }

        [HttpPost("{nPages}")]
        [Authorize(Roles = "Admin")]
        public IActionResult ScrapeUpdate(int nPages)
        {
            // Tramite Hangfire imposto l'esecuzione in background dato che lo scraping (in base alla quantità di pagine) potrebbe durare anche una o due ore
            BackgroundJob.Enqueue(() => ScrapeUpdateTask(nPages));

            return Accepted();
        }

        // Il metodo deve per forza essere public per essere eseguito in background quindi lo faccio ignorare da Swagger
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task ScrapeUpdateTask(int nPages)
        {
            try
            {
                await _mangaScraperService.Update(nPages);

                _logger.LogInformation("Operazione di scraping effettuata con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel tentativo di eseguire l'operazione di ottenimento/inserimento dati nel database, {ex}", ex);
            }
        }
    }
}
