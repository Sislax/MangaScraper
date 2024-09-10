using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaScraper
{
    public class Settings
    {
        public int nPagine { get; set; }
        public string baseUrl { get; set; }
        public string adBlockExtensionLocation { get; set; }
        public string nextPageParamArchive { get; set; }
        public string folderForImages { get; set; }
        public string empty { get; set; }
        public string apiUrl { get; set; }
        public string endPointOperate { get; set; }
        public string endPointUpdate { get; set; }

        public Settings(IConfigurationRoot configuration)
        {
            nPagine = configuration.GetSection("Settings").GetValue<int>("NPagine");
            baseUrl = configuration.GetSection("Settings").GetValue<string>("BaseUrl") ?? throw new Exception();
            adBlockExtensionLocation = configuration.GetSection("Settings").GetValue<string>("AdBlockExtensionLocation") ?? throw new Exception();
            nextPageParamArchive = configuration.GetSection("Settings").GetValue<string>("NextPageParamArchive") ?? throw new Exception();
            folderForImages = configuration.GetSection("Settings").GetValue<string>("FolderForImages") ?? throw new Exception();
            empty = configuration.GetSection("Settings").GetValue<string>("Empty") ?? throw new Exception();
            apiUrl = configuration.GetSection("ApiSettings").GetValue<string>("MangaScraperApiUrl") ?? throw new Exception();
            endPointOperate = configuration.GetSection("ApiSettings").GetValue<string>("EndPointOperate") ?? throw new Exception();
            endPointUpdate = configuration.GetSection("ApiSettings").GetValue<string>("EndPointUpdate") ?? throw new Exception();

        }
    }
}
