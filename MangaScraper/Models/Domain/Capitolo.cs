using System.ComponentModel.DataAnnotations.Schema;

namespace MangaScraper.Models.Domain
{
    [Table("Capitoli")]
    public class Capitolo
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NumCapitolo { get; set; }
        public int VolumeId { get; set; }
        public List<ImagePosition> ImgPositions { get; } = new List<ImagePosition>();
        public Volume Volume { get; } = null!;

        public Capitolo(string numCapitolo, int volumeId)
        {
            NumCapitolo = numCapitolo;
            VolumeId = volumeId;
        }
    }
}
