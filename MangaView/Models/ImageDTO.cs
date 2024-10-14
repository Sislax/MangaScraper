namespace MangaView.Models
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public string PathImg { get; set; }
        public int CapitoloId { get; set; }
        public CapitoloDTO Capitolo { get; } = null!;

        public ImageDTO(string pathImg, int capitoloId)
        {
            PathImg = pathImg;
            CapitoloId = capitoloId;
        }
    }
}
