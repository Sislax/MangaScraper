namespace MangaScraper.Data.Models.DTOs
{
	public class CapitoloDTO
	{
		public string NumCapitolo { get; set; }
		public List<ImageDTO> ImgPositions { get; } = new List<ImageDTO>();

		public CapitoloDTO(string numCapitolo, List<ImageDTO> imgPositions)
		{
			NumCapitolo = numCapitolo;
			ImgPositions = imgPositions;
		}
	}
}
