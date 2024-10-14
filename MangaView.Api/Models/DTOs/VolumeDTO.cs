namespace MangaView.Api.Models.DTOs
{
    public class VolumeDTO
    {
        public string NumVolume { get; set; }
        public List<CapitoloDTO> Capitoli { get; } = new List<CapitoloDTO>();

        public VolumeDTO(string numVolume, List<CapitoloDTO> capitoli)
        {
            NumVolume = numVolume;
            Capitoli = capitoli;
        }
    }
}
