using MangaScraper.Data.Models.Domain;
using MangaView.Api.Models.DTOs;

namespace MangaView.Api.Interfaces
{
    public interface IMangaService
    {
        public Task<IEnumerable<MangaDTO>> CreateMangaDTOs();
        public Task<VolumeDTO> CreateVolumeDTO(Volume volume);
        public Task<CapitoloDTO> CreateCapitoloDTO(Capitolo capitolo);
        public Task<ImageDTO> CreateImageDTO(ImagePosition imagePosition);
        public GenereDTO CreateGenereDTO(Genere genere);
        public Task<string> GetCopertinaMangaById(int id);
    }
}
