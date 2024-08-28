using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MangaScraper.Models.Settings;
using MangaScraper.Interfaces;
using MangaScraper.Services;
using MangaScraper.Data;
using Microsoft.EntityFrameworkCore;
using MangaScraper.Interfaces.RepoInterfaces;
using MangaScraper.Repositories;

namespace MangaScraper
{
    public class Program
    {
        public static void Main(string[] args)
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
                services.AddDbContext<AppDbContext>(
                    o => o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

                services.AddLogging(logging =>
                    logging.AddSimpleConsole(options =>
                    {
                        options.IncludeScopes = true;
                        //options.SingleLine = true;
                        options.TimestampFormat = "yyyy/MM/d HH:mm:ss:fff";
                    }));

                services.AddSingleton<ISeleniumService, SeleniumService>();

                //PER LE MIGRATION HO BISOGNO DI AGGIUNGERE QUESTI SERVIZI COME SCOPED INVECE CHE SINGLETON
                //ALTRIMENTI RICEVO UN ERRORE QUANDO CERCO DI CREARE UNA NUOVA MIGRATION IN QUANTO APPDBCONTEXT VIENE USATO COME SCOPED -> VERIFICARE LA CORRETTEZZA
                services.AddScoped<IMangaScraperService, MangaScraperService>();
                services.AddScoped<IMangaRepository, MangaRepository>();
                services.AddScoped<IGenereRepository, GenereRepository>();

                services.AddSingleton<Settings>();
                services.AddSingleton(configuration);
            })
            .Build();

            IMangaScraperService scraper = host.Services.GetRequiredService<IMangaScraperService>();

            scraper.Operate();

            Console.ReadKey();

            //TODO:.... AGGIUNGERE FUNZIONI DI AGGIORNAMENTO -> Se "empty" = true in appsettings.json eseguire metodo Operate() in quanto indica che il database è completamente vuoto
            //                                                  Se "empty" = false eseguire metodo Update() (DA CREARE) per aggiornare il database in quanto sono già presenti alcuni dati
            //                                                      Per il metodo Update() -> aggiungere campo nel db chiamato Data_Update e confrontarla con le ultime aggiunte sul sito
            //                                                      Eseguire l'update while(Data_Update < data nuova aggiunta)
        }
    }
}