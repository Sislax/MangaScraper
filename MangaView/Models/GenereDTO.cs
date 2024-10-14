using MangaView.Models.Utiles;

namespace MangaView.Models
{
    public class GenereDTO
    {
        public string NameId { get; set; } = null!;
        public GenereEnum GenereEnum { get; set; }
        public List<MangaDTO> Mangas { get; } = new List<MangaDTO>();
    }
}
