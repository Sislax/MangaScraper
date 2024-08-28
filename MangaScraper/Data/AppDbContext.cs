using MangaScraper.Data.ModelConfiguration;
using MangaScraper.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MangaScraper.Data
{
    public class AppDbContext : DbContext
    {
        public readonly DbContextOptions<AppDbContext> _options;
        public DbSet<Manga> Mangas { get; set; }
        public DbSet<Volume> Volumes { get; set; }
        public DbSet<Capitolo> Capitolos { get; set; }
        public DbSet<ImagePosition> ImagePositions { get; set; }
        public DbSet<Genere> Generes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            new MangaEntityTypeConfiguration().Configure(builder.Entity<Manga>());
            new VolumeEntityTypeConfiguration().Configure(builder.Entity<Volume>());
            new CapitoloEntityTypeConfiguration().Configure(builder.Entity<Capitolo>());
            new ImagePostionEntityTypeConfiguration().Configure(builder.Entity<ImagePosition>());
            new GenereEntityTypeConfiguration().Configure(builder.Entity<Genere>());
            new MangaGenereEntityTypeConfiguration().Configure(builder.Entity<MangaGenere>());
            base.OnModelCreating(builder);
        }
    }
}
