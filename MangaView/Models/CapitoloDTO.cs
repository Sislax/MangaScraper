namespace MangaView.Models
{
    public class CapitoloDTO
    {
        public int Id { get; set; }
        public string NumCapitolo { get; set; }
        public int VolumeId { get; set; }
        public List<ImageDTO> ImgPositions { get; } = new List<ImageDTO>();
        public VolumeDTO Volume { get; } = null!;

        public CapitoloDTO(string numCapitolo, int volumeId)
        {
            NumCapitolo = numCapitolo;
            VolumeId = volumeId;
        }
    }
}
