using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MangaScraper.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration(builder =>
            {
                builder.AddConfiguration(configuration);
            })
            .ConfigureServices(services =>
            {
                services.AddLogging(logging =>
                    logging.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        //options.SingleLine = true;
                        options.TimestampFormat = "yyyy/MM/d HH:mm:ss:fff";
                    }));

                services.AddSingleton<HttpRequestMangaScraperApi>();
                services.AddSingleton<Settings>();
                services.AddSingleton(configuration);
            })
            .Build();

            HttpRequestMangaScraperApi request = host.Services.GetRequiredService<HttpRequestMangaScraperApi>();

            using (HttpClient client = new HttpClient())
            {
                await request.RequestUpdate(client);
            }

            Console.ReadKey();
        }
    }
}