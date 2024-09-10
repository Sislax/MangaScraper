using MangaScraperApi.Models.Domain;

namespace MangaScraperApi.Interfaces.RepoInterfaces
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
