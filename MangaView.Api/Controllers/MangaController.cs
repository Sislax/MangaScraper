using MangaView.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaView.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        public readonly IMangaService _mangaService;
        public readonly ILogger<MangaController> _logger;

        public MangaController(IMangaService mangaService, ILogger<MangaController> logger)
        {
            _mangaService = mangaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetMangaDTOsAsync()
        {
            try
            {
                return Ok(await _mangaService.CreateMangaDTOs());
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire i mangaDTO. {ex}", ex);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCopertinaAsync(int id)
        {
            try
            {
                string copertinaPath = await _mangaService.GetCopertinaMangaById(id) + ".jpg";

                if (!System.IO.File.Exists(copertinaPath))
                {
                    return NotFound();
                }

                var fileStream = new FileStream(copertinaPath, FileMode.Open, FileAccess.Read);

                return File(fileStream, "image/jpeg");
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la copertina del manga. {ex}", ex);
                throw;
            }
        }
    }
}
