using Hangfire;
using MangaScraperApi.Interfaces.ServiceInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaScraperApi.Controllers
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
        public IActionResult ScrapeFromScratch(int nPages)
        {
            // Tramite Hangfire imposto l'esecuzione in background dato che lo scraping (in base alla quantità di pagine) potrebbe durare anche una o due ore
            BackgroundJob.Enqueue(() => ScrapeFromScratchTask(nPages));

            return Accepted();
        }

        [HttpPost]
        public IActionResult ScrapeUpdate()
        {
            // Tramite Hangfire imposto l'esecuzione in background dato che lo scraping (in base alla quantità di pagine) potrebbe durare anche una o due ore
            BackgroundJob.Enqueue(() => ScrapeUpdateTask());

            return Accepted();
        }

        // Il metodo deve per forza essere public per essere eseguito in background quindi lo faccio ignorare da Swagger
        [ApiExplorerSettings(IgnoreApi = true)]
        public void ScrapeFromScratchTask(int nPages)
        {
            try
            {
                _mangaScraperService.Operate(nPages);

                _logger.LogInformation("Operazione di scraping effettuata con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel tentativo di eseguire l'operazione di ottenimento/inserimento dati nel database, {ex}", ex);
            }
        }

        // Il metodo deve per forza essere public per essere eseguito in background quindi lo faccio ignorare da Swagger
        [ApiExplorerSettings(IgnoreApi = true)]
        public void ScrapeUpdateTask()
        {
            try
            {
                _mangaScraperService.Update();

                _logger.LogInformation("Operazione di scraping effettuata con successo");
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel tentativo di eseguire l'operazione di ottenimento/inserimento dati nel database, {ex}", ex);
            }
        }
    }
}
