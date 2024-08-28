using MangaScraper.Models.Domain;

namespace MangaScraper.Interfaces.RepoInterfaces
{
    public interface IGenereRepository : IDisposable
    {
        IEnumerable<Genere> GetGeneres();
        Genere GetGenereByName(string name);
        void InsertGenere(Genere imagePosition);
        void UpdateGenere(Genere imagePosition);
        void DeleteGenere(string name);
        void SaveChanges();
        void SaveChangesAsync();
    }
}
