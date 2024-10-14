using MangaScraper.Data.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaScraper.Data.Data.ModelConfiguration
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
