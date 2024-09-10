
namespace MangaScraperApi.Models.Settings
{
    public class Settings
    {
        public int nPagine { get; set; }
        public string baseUrl { get; set; }
        public string adBlockExtensionLocation { get; set; }
        public string nextPageParamArchive { get; set; }
        public string folderForImages { get; set; }

        public Settings(IConfigurationRoot configuration)
        {
            nPagine = configuration.GetSection("Settings").GetValue<int>("nPagine");
            baseUrl = configuration.GetSection("Settings").GetValue<string>("baseUrl") ?? throw new Exception();
            adBlockExtensionLocation = configuration.GetSection("Settings").GetValue<string>("adBlockExtensionLocation") ?? throw new Exception();
            nextPageParamArchive = configuration.GetSection("Settings").GetValue<string>("nextPageParamArchive") ?? throw new Exception();
            folderForImages = configuration.GetSection("Settings").GetValue<string>("folderForImages") ?? throw new Exception();
        }
    }
}
