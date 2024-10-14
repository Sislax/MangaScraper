using Microsoft.Extensions.Configuration;

namespace MangaScraperApi
{
    public class Settings
    {
        public int NPagine { get; set; }
        public string ApiUrl { get; set; }
        public string EndPointUpdate { get; set; }

        public Settings(IConfigurationRoot configuration)
        {
            NPagine = configuration.GetSection("Settings").GetValue<int>("NPagine");
            ApiUrl = configuration.GetSection("ApiSettings").GetValue<string>("MangaScraperApiUrl") ?? throw new Exception();
            EndPointUpdate = configuration.GetSection("ApiSettings").GetValue<string>("EndPointUpdate") ?? throw new Exception();
        }
    }
}
