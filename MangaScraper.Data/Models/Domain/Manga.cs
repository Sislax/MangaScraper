using System.ComponentModel.DataAnnotations.Schema;

namespace MangaScraper.Data.Models.Domain
{
    [Table("Mangas")]
    public class Manga
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CopertinaUrl { get; set; }
        public string Url { get; set; }
        public string Tipo { get; set; }
        public string Stato { get; set; }
        public string Autore { get; set; }
        public string Artista { get; set; }
        public string Trama { get; set; }
        public List<Genere> Generi { get; set; } = new List<Genere>();
        public List<MangaGenere> MangaGenereList { get; set; } = new List<MangaGenere>();
        public List<Volume> Volumi { get; set; } = new List<Volume>();

        public Manga(string nome, string copertinaUrl, string url, string tipo, string stato, string autore, string artista, string trama)
        {
            Nome = nome;
            CopertinaUrl = copertinaUrl;
            Url = url;
            Tipo = tipo;
            Stato = stato;
            Autore = autore;
            Artista = artista;
            Trama = trama;
        }
    }
}
