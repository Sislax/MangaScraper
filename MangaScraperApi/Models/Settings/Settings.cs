
namespace MangaScraperApi.Models.Settings
{
    public class Settings
    {
        public string BaseUrl { get; set; }
        public string AdBlockExtensionLocation { get; set; }
        public string FolderForImages { get; set; }

        public Settings(IConfigurationRoot configuration)
        {
            BaseUrl = configuration.GetSection("Settings").GetValue<string>("baseUrl") ?? throw new Exception();
            AdBlockExtensionLocation = configuration.GetSection("Settings").GetValue<string>("adBlockExtensionLocation") ?? throw new Exception();
            FolderForImages = configuration.GetSection("Settings").GetValue<string>("folderForImages") ?? throw new Exception();
        }
    }
}
