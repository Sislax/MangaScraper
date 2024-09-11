﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;

namespace MangaScraper
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

            string empty = host.Services.GetRequiredService<Settings>().Empty;

            using(HttpClient client = new HttpClient())
            {
                if (empty == "true")
                {
                    await request.RequestOperate(client);
                }
                else
                {
                    await request.RequestUpdate(client);
                }
            }

            Console.ReadKey();

            //TODO:.... IMPLEMETARE IL METODO UPDATE() NELL'API
            //          IMPLEMENTARE UI CON BLAZOR
        }
    }
}