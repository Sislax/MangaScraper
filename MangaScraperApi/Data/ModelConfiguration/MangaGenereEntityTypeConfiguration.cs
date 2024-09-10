using MangaScraperApi.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaScraperApi.Data.ModelConfiguration
{
    //Classe di configurazione necessaria perchè EF nella migration imposta come chiave MangaId + un campo generato automaticamente chiamato GenereTempId
    public class MangaGenereEntityTypeConfiguration : IEntityTypeConfiguration<MangaGenere>
    {
        public void Configure(EntityTypeBuilder<MangaGenere> builder)
        {
            //Primary Key
            builder.HasKey(mg => new
            {
                mg.MangaId,
                mg.GenereId
            });
        }
    }
}
