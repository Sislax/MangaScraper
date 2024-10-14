namespace MangaView.Api.Models.DTOs
{
    public class MangaDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CopertinaPath { get; set; }
        public string Tipo { get; set; }
        public string Stato { get; set; }
        public string Autore { get; set; }
        public string Artista { get; set; }
        public string Trama { get; set; }
        public List<GenereDTO> Generi { get; set; } = new List<GenereDTO>();
        public List<VolumeDTO> Volumi { get; set; } = new List<VolumeDTO>();

        public MangaDTO(int id, string nome, string copertinaPath, string tipo, string stato, string autore, string artista, string trama, List<GenereDTO> generi, List<VolumeDTO> volumi)
        {
            Id = id;
            Nome = nome;
            CopertinaPath = copertinaPath;
            Tipo = tipo;
            Stato = stato;
            Autore = autore;
            Artista = artista;
            Trama = trama;
            Generi = generi;
            Volumi = volumi;
        }

        public MangaDTO(int id, string nome, string copertinaPath, string tipo, string stato, string autore, string artista, string trama, List<GenereDTO> generi)
        {
            Id = id;
            Nome = nome;
            CopertinaPath = copertinaPath;
            Tipo = tipo;
            Stato = stato;
            Autore = autore;
            Artista = artista;
            Trama = trama;
            Generi = generi;
        }
    }
}
