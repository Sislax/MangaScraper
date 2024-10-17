using MangaView.Api.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Metadata;
using SixLabors.ImageSharp.Processing;

namespace MangaView.Api.Services
{
	public class ImageSharpService : IImageSharpService
	{
		public byte[] ResizeImage(FileStream fileStream, int width, int height)
		{
			Image image = Image.Load(fileStream);

			image.Mutate(i => i.Resize(new ResizeOptions
			{
				Size = new Size(width, height),
				Mode = ResizeMode.Max,
				Sampler = KnownResamplers.Lanczos8,
				Compand = true
			}));

			image.Metadata.ResolutionUnits = PixelResolutionUnit.PixelsPerInch;
			image.Metadata.HorizontalResolution = 500;
			image.Metadata.VerticalResolution = 500;

			MemoryStream memoryStream = new MemoryStream();
			image.Save(memoryStream, new JpegEncoder { Quality = 100 });

			return memoryStream.ToArray();
		}
	}
}
