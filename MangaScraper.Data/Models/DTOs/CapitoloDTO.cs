using System.Text.Json.Serialization;

namespace MangaScraper.Data.Models.DTOs
{
	public class CapitoloDTO
	{
		public int Id { get; set; }
		public string NumCapitolo { get; set; }
		public List<ImageDTO> ImgPositions { get; } = new List<ImageDTO>();

        [JsonConstructor]
        public CapitoloDTO(int id, string numCapitolo, List<ImageDTO> imgPositions)
		{
			Id = id;
			NumCapitolo = numCapitolo;
			ImgPositions = imgPositions;
		}

		#pragma warning disable CS8618
        public CapitoloDTO()
        {
            
        }
		#pragma warning restore CS8618
    }
}
