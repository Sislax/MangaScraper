using MangaScraper.Data;
using MangaScraper.Interfaces.RepoInterfaces;
using MangaScraper.Models.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MangaScraper.Repositories
{
    public class MangaRepository : IMangaRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction = null!;
        private readonly ILogger<MangaRepository> _logger;
        private bool disposed = false;

        public MangaRepository(AppDbContext context, ILogger<MangaRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Manga> GetMangas()
        {
            try
            {
                return _context.Mangas.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la lista dei manga. {ex}", ex);
                throw;
            }
        }

        public Manga GetMangaById(int id)
        {
            try
            {
                return _context.Mangas.Find(id) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile resitutire il manga con id: {id}. {ex}", id, ex);
                throw;
            }
        }

        public void InsertManga(Manga manga)
        {
            try
            {
                _context.Mangas.Add(manga);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile inserire il manga con id: {manga.Id}. {ex}", manga.Id, ex);
                throw;
            }
        }

        public void InsertRangeAsyncManga(IEnumerable<Manga> mangas)
        {
            try
            {
                _context.AddRangeAsync(mangas);
            }
            catch(Exception ex)
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
                _logger.LogError("ERRORE: impossibile aggiornare il manga con id: {manga.Id}. {ex}", manga.Id, ex);
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
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
