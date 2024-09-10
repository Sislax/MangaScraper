using MangaScraperApi.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraperApi.Data.ModelConfiguration
{
    public class CapitoloEntityTypeConfiguration : IEntityTypeConfiguration<Capitolo>
    {
        public void Configure(EntityTypeBuilder<Capitolo> builder)
        {
            //Primary Key
            builder.HasKey(c => c.Id);

            //Navigation OneToMany Capitolo -> ImagePosition
            builder
                .HasMany(c => c.ImgPositions)
                .WithOne(i => i.Capitolo)
                .HasForeignKey(c => c.CapitoloId);

            builder.Property(c => c.Id);
            builder.Property(c => c.NumCapitolo);
            builder.Property(c => c.VolumeId);
        }
    }
}
