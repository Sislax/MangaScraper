using MangaScraper.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraper.Data.ModelConfiguration
{
    public class VolumeEntityTypeConfiguration : IEntityTypeConfiguration<Volume>
    {
        public void Configure(EntityTypeBuilder<Volume> builder)
        {
            //Primary Key
            builder.HasKey(v => v.Id);

            //Navigation OneToMany Volume -> Capitolo
            builder
                .HasMany(v => v.Capitoli)
                .WithOne(v => v.Volume)
                .HasForeignKey(v => v.VolumeId);

            builder.Property(v => v.Id);
            builder.Property(v => v.NumVolume);
            builder.Property(v => v.MangaId);
        }
    }
}
