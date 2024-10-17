using MangaScraper.Data.Models.Domain;
using MangaScraper.Data.Models.DTOs;

namespace MangaView.Api.Interfaces
{
    public interface IMangaService
    {
        public Task<IEnumerable<MangaDTO>> CreateMangaDTOs();
        public VolumeDTO CreateVolumeDTO(Volume volume);
        public CapitoloDTO CreateCapitoloDTO(Capitolo capitolo);
        public ImageDTO CreateImageDTO(ImagePosition imagePosition);
        public GenereDTO CreateGenereDTO(Genere genere);
        public Task<MangaDTO> CreateMangaDTOWithData(int id);
        public Task<string> GetCopertinaMangaById(int id);
    }
}
