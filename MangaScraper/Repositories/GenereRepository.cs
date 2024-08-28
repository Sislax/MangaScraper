using MangaScraper.Data;
using MangaScraper.Interfaces.RepoInterfaces;
using MangaScraper.Models.Domain;
using Microsoft.Extensions.Logging;

namespace MangaScraper.Repositories
{
    public class GenereRepository : IGenereRepository, IDisposable
    {
        public readonly AppDbContext _context;
        public readonly ILogger<GenereRepository> _logger;
        private bool disposed = false;

        public GenereRepository(AppDbContext context, ILogger<GenereRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Genere> GetGeneres()
        {
            try
            {
                return _context.Generes.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile restituire la lista dei generi. {ex}", ex);
                throw;
            }
        }

        public Genere GetGenereByName(string name)
        {
            try
            {
                return _context.Generes.Find(name) ?? throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile resitutire il genere: {name}. {ex}", name, ex);
                throw;
            }
        }

        public void InsertGenere(Genere genere)
        {
            try
            {
                _context.Generes.Add(genere);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile inserire il genere: {genere.Name}. {ex}", genere.NameId, ex);
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

        public void SaveChangesAsync()
        {
            try
            {
                _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: impossibile salvare i cambiamenti effettuati sul database. {ex}", ex);
                throw;
            }
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
