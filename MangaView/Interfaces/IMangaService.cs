using MangaView.Models;

namespace MangaView.Interfaces
{
	public interface IMangaService
	{
		public Task<List<MangaDTO>> GetAllMangas();
	}
}
