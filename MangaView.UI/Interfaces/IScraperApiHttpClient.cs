using MangaScraper.Data.Models.DTOs;

namespace MangaView.UI.Interfaces
{
    public interface IScraperApiHttpClient
    {
        Task<List<MangaDTO>> GetMangaDTOs();
        Task<MangaDTO> GetMangaDTOWithAllData(int mangaId);
        Task<CapitoloDTO> GetCapitoloDTOWithData(int capitoloId);
        string GetCurrentImage(int imageId);
        string GetCopertina(int id);
    }
}
