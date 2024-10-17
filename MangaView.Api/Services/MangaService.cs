using MangaScraper.Data.Interfaces;
using MangaScraper.Data.Models.Domain;
using MangaScraper.Data.Models.DTOs;
using MangaView.Api.Interfaces;

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

        public VolumeDTO CreateVolumeDTO(Volume volume)
        {
            try
            {
                List<CapitoloDTO> capitoloDTOList = new List<CapitoloDTO>();

                foreach(Capitolo capitolo in volume.Capitoli)
                {
                    capitoloDTOList.Add(CreateCapitoloDTO(capitolo));
                }

                return new VolumeDTO(volume.NumVolume, capitoloDTOList);
            }
            catch (Exception ex)
            {
				_logger.LogError("ERRORE: impossibile creare il VolumeDTO. {ex}", ex);
				throw;
			}
        }

        public CapitoloDTO CreateCapitoloDTO(Capitolo capitolo)
        {
			try
			{
				List<ImageDTO> imageDTOList = new List<ImageDTO>();

                foreach(ImagePosition imgPosition in capitolo.ImgPositions)
                {
                    imageDTOList.Add(CreateImageDTO(imgPosition));
                }

                return new CapitoloDTO(capitolo.NumCapitolo, imageDTOList);
			}
			catch (Exception ex)
			{
				_logger.LogError("ERRORE: impossibile creare il CapitoloDTO. {ex}", ex);
				throw;
			}
		}

        public GenereDTO CreateGenereDTO(Genere genere)
        {
            try
            {
                return new GenereDTO(genere.NameId);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile creare il GenereDTO dal genere {genere.NameId}. {ex}", genere.NameId, ex);
                throw;
            }
        }

        public ImageDTO CreateImageDTO(ImagePosition imagePosition)
        {
			try
			{
                return new ImageDTO(imagePosition.Id);
			}
			catch (Exception ex)
			{
				_logger.LogError("ERRORE: impossibile creare l'ImageDTO. {ex}", ex);
				throw;
			}
		}

        public async Task<MangaDTO> CreateMangaDTOWithData(int id)
        {
            try
            {
                Manga manga = await _appRepository.GetMangaWithAllDataAsync(id);

                List<VolumeDTO> volumeDTOList = new List<VolumeDTO>();

                foreach(Volume volume in manga.Volumi)
                {
                    volumeDTOList.Add(CreateVolumeDTO(volume));
                }

				List<GenereDTO> genereDTOList = new List<GenereDTO>();

				foreach (Genere genere in manga.Generi)
				{
					genereDTOList.Add(CreateGenereDTO(genere));
				}

				return new MangaDTO(manga.Id, manga.Nome, manga.CopertinaUrl, manga.Tipo, manga.Stato, manga.Autore, manga.Artista, manga.Trama, genereDTOList, volumeDTOList);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile creare il MangaDTO del manga con id {id}. {ex}", id, ex);
                throw;
            }
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
