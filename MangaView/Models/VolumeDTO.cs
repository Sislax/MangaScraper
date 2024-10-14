namespace MangaView.Models
{
    public class VolumeDTO
    {
        public int Id { get; set; }
        public string NumVolume { get; set; }
        public int MangaId { get; set; }
        public MangaDTO Manga { get; } = null!;
        public List<CapitoloDTO> Capitoli { get; } = new List<CapitoloDTO>();

        public VolumeDTO(string numVolume, int mangaId)
        {
            NumVolume = numVolume;
            MangaId = mangaId;
        }
    }
}
