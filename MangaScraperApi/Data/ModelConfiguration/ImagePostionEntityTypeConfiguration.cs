using MangaScraperApi.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraperApi.Data.ModelConfiguration
{
    public class ImagePostionEntityTypeConfiguration : IEntityTypeConfiguration<ImagePosition>
    {
        public void Configure(EntityTypeBuilder<ImagePosition> builder)
        {
            //Primary Key
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id);
            builder.Property(i => i.PathImg);
            builder.Property(i => i.CapitoloId);
        }
    }
}
