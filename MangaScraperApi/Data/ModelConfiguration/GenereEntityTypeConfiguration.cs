using MangaScraperApi.Models.Domain;
using MangaScraperApi.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraperApi.Data.ModelConfiguration
{
    public class GenereEntityTypeConfiguration : IEntityTypeConfiguration<Genere>
    {
        public void Configure(EntityTypeBuilder<Genere> builder)
        {
            //Primary Key
            builder.HasKey(g => g.NameId);

            //Navigation ManyToMany Genere -> Manga
            builder
                .HasMany(g => g.Mangas)
                .WithMany(m => m.Generi)
                .UsingEntity<MangaGenere>(
                    r => r.HasOne(mg => mg.Genere).WithMany(g => g.MangaGenereList).HasForeignKey(mg => mg.GenereId));

            builder.Property(g => g.NameId).IsRequired();
            builder.Property(g => g.GenereEnum)
                .HasConversion<int>();

            builder.HasData(
                Enum.GetValues(typeof(GenereEnum))
                .Cast<GenereEnum>()
                .Select(ge => new Genere()
                {
                    GenereEnum = ge,
                    NameId = ge.ToString()
                })
            );
        }
    }
}
