namespace MangaScraper.Data.Models.Utiles
{
    public class MangaToCompare
    {
        public string Nome { get; set; } = null!;
        public int NCapitoli { get; set; }

        public MangaToCompare(string nome, int nCapitoli)
        {
            Nome = nome;
            NCapitoli = nCapitoli;
        }

        public MangaToCompare() { }
    }
}
