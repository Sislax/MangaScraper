﻿using MangaScraperApi.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaScraperApi.Models.Domain
{
    [Table("Generi")]
    public class Genere
    {
        public string NameId { get; set; } = null!;
        public GenereEnum GenereEnum { get; set; }
        public List<MangaGenere> MangaGenereList { get; } = new List<MangaGenere>();
        public List<Manga> Mangas { get; } = new List<Manga>();
    }
}
