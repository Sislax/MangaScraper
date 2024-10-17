namespace MangaView.Api.Interfaces
{
	public interface IImageSharpService
	{
		public byte[] ResizeImage(FileStream fileStream, int width, int height);
	}
}
