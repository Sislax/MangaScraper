using MangaScraper.Data.Models.Domain;
using MangaView.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MangaView.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        public readonly IMangaService _mangaService;
        public readonly IImageSharpService _imageSharpService;
        public readonly ILogger<MangaController> _logger;

        public MangaController(IMangaService mangaService, IImageSharpService imageSharpService, ILogger<MangaController> logger)
        {
            _mangaService = mangaService;
            _imageSharpService = imageSharpService;
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
        public async Task<IActionResult> GetMangaDTOWithAllDataAsync(int id)
        {
            try
            {
                return Ok(await _mangaService.CreateMangaDTOWithData(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire il mangaDTO. {ex}", ex);
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

                using (FileStream fileStream = new FileStream(copertinaPath, FileMode.Open, FileAccess.Read))
                {
					return File(_imageSharpService.ResizeImage(fileStream, 640, 920), "image/jpeg");
				}
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la copertina del manga. {ex}", ex);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCapitoloDTOWithDataAsync(int id)
        {
            try
            {
                Capitolo capitolo = await _mangaService.GetCapitoloByIdAsync(id);

                return Ok(_mangaService.CreateCapitoloDTO(capitolo));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire il capitoloDTO dal capitolo con id: {id}. {ex}", id, ex);
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetImageFromImageId(int id)
        {
            try
            {
                string imagePath = await _mangaService.GetPathImgAsync(id);

				if (!System.IO.File.Exists(imagePath))
				{
					return NotFound();
				}

                FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
				
				return File(fileStream, "image/jpeg");
				

			}
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire l'immagine con id: {id}. {ex}", id, ex);
                throw;
            }
        }
    }
}
