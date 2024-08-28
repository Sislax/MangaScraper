using MangaScraper.Models.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace MangaScraper.Interfaces.RepoInterfaces
{
    public interface IMangaRepository : IDisposable
    {
        IEnumerable<Manga> GetMangas();
        Manga GetMangaById(int id);
        void InsertManga(Manga imagePosition);
        void InsertRangeAsyncManga(IEnumerable<Manga> mangas);
        void UpdateManga(Manga imagePosition);
        void DeleteManga(int id);
        Task<IDbContextTransaction> BeginTransactionAsync();
        void SaveChanges();
        Task SaveChangesAsync();

    }
}
