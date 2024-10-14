namespace MangaView.Api.Models.DTOs
{
    public class ImageDTO
    {
        public FileStream Image { get; set; }

        public ImageDTO(FileStream image)
        {
            Image = image;
        }
    }
}
