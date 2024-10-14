using System.ComponentModel.DataAnnotations.Schema;

namespace MangaScraper.Data.Models.Domain
{
    [Table("ImagePositions")]
    public class ImagePosition
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PathImg { get; set; }
        public int CapitoloId { get; set; }
        public Capitolo Capitolo { get; } = null!;

        public ImagePosition(string pathImg, int capitoloId)
        {
            PathImg = pathImg;
            CapitoloId = capitoloId;
        }
    }
}
