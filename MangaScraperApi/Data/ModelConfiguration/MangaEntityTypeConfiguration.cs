using MangaScraperApi.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraperApi.Data.ModelConfiguration
{
    public class MangaEntityTypeConfiguration : IEntityTypeConfiguration<Manga>
    {
        public void Configure(EntityTypeBuilder<Manga> builder)
        {
            //Primary Key
            builder.HasKey(m => m.Id);

            //Navigation OneToMany Manga -> Volume
            builder
                .HasMany(m => m.Volumi)
                .WithOne(v => v.Manga)
                .HasForeignKey(m => m.MangaId)
                .IsRequired(false);

            //Navigation ManyToMany Manga -> Genere
            builder
                .HasMany(m => m.Generi)
                .WithMany(g => g.Mangas)
                .UsingEntity<MangaGenere>(
                    l => l.HasOne(mg => mg.Manga).WithMany(m => m.MangaGenereList).HasForeignKey(mg => mg.MangaId));

            builder.Property(m => m.Id);
            builder.Property(m => m.Nome);
            builder.Property(m => m.Url);
            builder.Property(m => m.Tipo);
            builder.Property(m => m.Stato);
            builder.Property(m => m.Autore);
            builder.Property(m => m.Artista);
            builder.Property(m => m.Trama);
        }
    }
}
