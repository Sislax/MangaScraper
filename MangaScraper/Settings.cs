using Microsoft.Extensions.Configuration;

namespace MangaScraper
{
    public class Settings
    {
        public int NPagine { get; set; }
        public string Empty { get; set; }
        public string ApiUrl { get; set; }
        public string EndPointOperate { get; set; }
        public string EndPointUpdate { get; set; }

        public Settings(IConfigurationRoot configuration)
        {
            NPagine = configuration.GetSection("Settings").GetValue<int>("NPagine");
            Empty = configuration.GetSection("Settings").GetValue<string>("Empty") ?? throw new Exception();
            ApiUrl = configuration.GetSection("ApiSettings").GetValue<string>("MangaScraperApiUrl") ?? throw new Exception();
            EndPointOperate = configuration.GetSection("ApiSettings").GetValue<string>("EndPointOperate") ?? throw new Exception();
            EndPointUpdate = configuration.GetSection("ApiSettings").GetValue<string>("EndPointUpdate") ?? throw new Exception();
        }
    }
}
