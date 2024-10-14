namespace MangaView.Models
{
	public class Settings
	{
		public string ApiUrl { get; set; }

		public Settings(IConfigurationRoot clientConfiguration)
        {
			ApiUrl = clientConfiguration.GetSection("ApiSettings").GetValue<string>("MangaScraperApiUrl") ?? throw new Exception("MangaScraperApiUrl non trovata in configuration.");
        }
    }
}
