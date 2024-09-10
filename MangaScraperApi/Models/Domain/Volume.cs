using System.ComponentModel.DataAnnotations.Schema;

namespace MangaScraperApi.Models.Domain
{
    [Table("Volumi")]
    public class Volume
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NumVolume { get; set; }
        public int MangaId { get; set; }
        public Manga Manga { get; } = null!;
        public List<Capitolo> Capitoli {  get; } = new List<Capitolo>();

        public Volume(string numVolume, int mangaId)
        {
            NumVolume = numVolume;
            MangaId = mangaId;
        }
    }
}
