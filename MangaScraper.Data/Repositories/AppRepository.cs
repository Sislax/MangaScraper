using MangaScraper.Data.Data;
using MangaScraper.Data.Interfaces;
using MangaScraper.Data.Models.Domain;
using MangaScraper.Data.Models.Utiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace MangaScraper.Data.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction = null!;
        private readonly ILogger<AppRepository> _logger;

        public AppRepository(AppDbContext context, ILogger<AppRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Manga>> GetMangasAsync()
        {
            try
            {
                return await _context.Mangas.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la lista dei manga. {ex}", ex);
                throw;
            }
        }

        public async Task<string> GetCopertinaMangaByIdAsync(int idManga)
        {
            try
            {
                Manga manga = await _context.Mangas.SingleAsync(m => m.Id == idManga);

                return manga.CopertinaUrl + ".jpg";
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la lista dei manga. {ex}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Manga>> GetMangasWithAllDataAsync()
        {
            try
            {
                return await _context.Mangas
                    .Include(m => m.Volumi)
                    .ThenInclude(v => v.Capitoli)
                    .ThenInclude(c => c.ImgPositions)
                    .Include(m => m.Generi)
                    .ToListAsync();
            }
            catch(Exception ex)
            {
                _logger.LogInformation("ERRORE: impossibile restituire i manga con tutti i suoi dati. {ex}", ex);
                throw;
            }
        }

        public async Task<Manga> GetMangaWithAllDataAsync(int id)
        {
            try
            {
                return await _context.Mangas
                    .Include(m => m.Volumi)
                    .ThenInclude(v => v.Capitoli)
                    .ThenInclude(c => c.ImgPositions)
                    .Include(m => m.Generi)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ERRORE: impossibile restituire i manga con tutti i suoi dati. {ex}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Manga>> GetMangasWithGeneriAsync()
        {
            try
            {
                return await _context.Mangas
                    .Include(m => m.Generi)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ERRORE: impossibile restituire i manga con tutti i suoi dati. {ex}", ex);
                throw;
            }
        }

        public async Task<Manga> GetMangaWithGeneriAsync(string nomeManga)
        {
            try
            {
                return await _context.Mangas
                    .Include(m => m.Generi)
                    .FirstAsync(m => m.Nome == nomeManga);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ERRORE: impossibile restituire i manga con tutti i suoi dati. {ex}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Genere>> GetGeneresAsync()
        {
            try
            {
                return await _context.Generes.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la lista dei generi. {ex}", ex);
                throw;
            }
        }

        public async Task<Genere> GetGenereByNameAsync(string name)
        {
            try
            {
                return await _context.Generes.FindAsync(name) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile resitutire il genere: {name}. {ex}", name, ex);
                throw;
            }
        }

        public async Task<Manga> GetMangaByIdAsync(int id)
        {
            try
            {
                return await _context.Mangas.FindAsync(id) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile resitutire il manga con id: {id}. {ex}", id, ex);
                throw;
            }
        }

        public async Task<Manga> GetMangaByNameAsync(string name)
        {
            try
            {
                return await _context.Mangas.FirstOrDefaultAsync(m => m.Nome == name) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile resitutire il manga con nome: {name}. {ex}", name, ex);
                throw;
            }
        }

        public async Task<IEnumerable<MangaToCompare>> GetCapitoliCountForEachMangaAsync()
        {
            try
            {
                if (!_context.Mangas.Any())
                {
                    return new List<MangaToCompare>();
                }

                return await _context.Mangas
                    .Select(m => new MangaToCompare
                    {
                        Nome = m.Nome,
                        NCapitoli = m.Volumi
                            .SelectMany(v => v.Capitoli)
                            .Count()

                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la lista dei manga con il numero dei loro capitoli. {ex}", ex);
                throw;
            }
        }

        public async Task<Capitolo> GetCapitoloWithDataByIdAsync(int capitoloId)
        {
            try
            {
                return await _context.Capitolos
                        .Include(c => c.ImgPositions.OrderBy(i => i.Id))
                        .AsSplitQuery()
                        .SingleAsync(c => c.Id == capitoloId);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile ottenere il path dell'immagine dal capitoloId: {capitoloId}. {ex}", capitoloId, ex);
                throw;
            }
        }

        public async Task<string> GetPathImageByIdAsync(int imgId)
        {
            try
            {
                ImagePosition img =  await _context.ImagePositions.SingleAsync(i => i.Id == imgId);

                return img.PathImg;
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire il percorso dell'immagine con id {imgId}. {ex}", imgId, ex);
                throw;
            }
        }

        public async Task AddMangaAsync(Manga manga)
        {
            try
            {
                await _context.Mangas.AddAsync(manga);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile inserire il manga con id: {manga.Id}. {ex}", manga.Id, ex);
                throw;
            }
        }

        public async Task AddGenereAsync(Genere genere)
        {
            try
            {
                await _context.Generes.AddAsync(genere);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile inserire il genere: {genere.Name}. {ex}", genere.NameId, ex);
                throw;
            }
        }

        public async Task AddRangeAsyncManga(IEnumerable<Manga> mangas)
        {
            try
            {
                await _context.AddRangeAsync(mangas);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile inserire la lista di manga. {ex}", ex);
                throw;
            }
        }

        public void UpdateManga(Manga manga)
        {
            try
            {
                _context.Mangas.Update(manga);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile aggiornare/inserire il manga con id: {manga.Id}. {ex}", manga.Id, ex);
                throw;
            }
        }

        public void UpdateGenere(Genere genere)
        {
            try
            {
                _context.Generes.Update(genere);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile aggiornare il genere con id: {genere.Id}. {ex}", genere, ex);
                throw;
            }
        }

        public void UpdateRangeManga(IEnumerable<Manga> mangas)
        {
            try
            {
                _context.Mangas.UpdateRange(mangas);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile aggiornare/inserire i manga. {ex}", ex);
                throw;
            }
        }

        public void DeleteManga(int id)
        {
            try
            {
                Manga manga = _context.Mangas.Find(id) ?? throw new Exception();
                _context.Mangas.Remove(manga);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile eliminare il manga con id: {id}. {ex}", id, ex);
                throw;
            }
        }

        public void DeleteGenere(string name)
        {
            try
            {
                Genere genere = _context.Generes.Find(name) ?? throw new Exception();
                _context.Generes.Remove(genere);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile eliminare il genere: {name}. {ex}", name, ex);
                throw;
            }
        }

        public void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile salvare i cambiamenti effettuati sul database. {ex}", ex);
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile salvare i cambiamenti effettuati sul database. {ex}", ex);
                throw;
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            try
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                return _transaction;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: Problemi nella creazione transazione. {ex}", ex);
                throw;
            }
        }
    }
}