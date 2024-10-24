
using System.Text.Json.Serialization;

namespace MangaScraper.Data.Models.DTOs
{
	public class VolumeDTO
	{
		public string NumVolume { get; set; }
		public List<CapitoloDTO> Capitoli { get; } = new List<CapitoloDTO>();

        [JsonConstructor]
        public VolumeDTO(string numVolume, List<CapitoloDTO> capitoli)
		{
			NumVolume = numVolume;
			Capitoli = capitoli;
		}

		#pragma warning disable CS8618
        public VolumeDTO()
		{

		}
		#pragma warning restore CS8618
    }
}
