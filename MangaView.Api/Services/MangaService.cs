using MangaScraper.Data.Interfaces;
using MangaScraper.Data.Models.Domain;
using MangaView.Api.Interfaces;
using MangaView.Api.Models.DTOs;

namespace MangaView.Api.Services
{
    public class MangaService : IMangaService
    {
        public readonly IAppRepository _appRepository;
        public readonly ILogger<MangaService> _logger;

        public MangaService(IAppRepository appRepository, ILogger<MangaService> logger)
        {
            _appRepository = appRepository;
            _logger = logger;
        }

        public async Task<VolumeDTO> CreateVolumeDTO(Volume volume)
        {
            throw new NotImplementedException();
        }

        public async Task<CapitoloDTO> CreateCapitoloDTO(Capitolo capitolo)
        {
            throw new NotImplementedException();
        }

        public GenereDTO CreateGenereDTO(Genere genere)
        {
            try
            {
                return new GenereDTO(genere.NameId);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile creare il GenereDTO dal genere {genere.NameId}", genere.NameId);
                throw;
            }
        }

        public async Task<ImageDTO> CreateImageDTO(ImagePosition imagePosition)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<MangaDTO>> CreateMangaDTOs()
        {
            try
            {
                List<Manga> mangaWithGeneriList = (List<Manga>)await _appRepository.GetMangasWithGeneriAsync();

                List<MangaDTO> mangaDTOList = new List<MangaDTO>();

                foreach (Manga manga in mangaWithGeneriList)
                {
                    List<GenereDTO> genereDTOList = new List<GenereDTO>();

                    foreach (Genere genere in manga.Generi)
                    {
                        genereDTOList.Add(new GenereDTO(genere.NameId));
                    }

                    mangaDTOList.Add(new MangaDTO(manga.Id, manga.Nome, manga.CopertinaUrl, manga.Tipo, manga.Stato, manga.Autore, manga.Artista, manga.Trama, genereDTOList));
                }

                return mangaDTOList;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile creare i DTO dei manga. {ex}", ex);
                throw;
            }
        }

        public async Task<string> GetCopertinaMangaById(int id)
        {
            try
            {
                return await _appRepository.GetCopertinaMangaByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile ottenere il percorso della copertina del manga con id: {id}. {ex}", id, ex);
                throw;
            }
        }
    }

}
