﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MangaScraper.Data.Models.Domain
{
    [Table("MangaGenere")]
    public class MangaGenere
    {
        public int MangaId { get; set; }
        public string GenereId { get; set; } = null!;
        public Manga Manga { get; set; } = null!;
        public Genere Genere { get; set; } = null!;

        public MangaGenere()
        {

        }
        public MangaGenere(int mangaId, string genereId)
        {
            MangaId = mangaId;
            GenereId = genereId;
        }
    }
}
