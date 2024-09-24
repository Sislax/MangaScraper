using MangaScraperApi.Models.Domain;
using MangaScraperApi.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace MangaScraperApi.Interfaces.RepoInterfaces
{
    public interface IAppRepository
    {
        public Task<IEnumerable<Manga>> GetMangasAsync();
        public Task<Manga> GetMangaByIdAsync(int id);
        public Task<Manga> GetMangaByNameAsync(string name);
        public Task<IEnumerable<Genere>> GetGeneresAsync();
        public Task<Genere> GetGenereByNameAsync(string name);
        public Task<IEnumerable<MangaToCompare>> GetCapitoliCountForEachMangaAsync();
        public Task AddMangaAsync(Manga imagePosition);
        public Task AddGenereAsync(Genere genere);
        public Task AddRangeAsyncManga(IEnumerable<Manga> mangas);
        public void UpdateManga(Manga imagePosition);
        public void UpdateGenere(Genere genere);
        public void UpdateRangeManga(IEnumerable<Manga> mangas);
        public void DeleteManga(int id);
        public void DeleteGenere(string name);
        public Task<IDbContextTransaction> BeginTransactionAsync();
        public void SaveChanges();
        public Task SaveChangesAsync();
    }
}
