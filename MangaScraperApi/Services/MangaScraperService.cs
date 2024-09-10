using MangaScraperApi.Interfaces.ServiceInterfaces;
using MangaScraperApi.Interfaces.RepoInterfaces;
using MangaScraperApi.Models.Domain;
using MangaScraperApi.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;

namespace MangaScraper.Services
{
    public class MangaScraperService : IMangaScraperService
    {
        private readonly ISeleniumService _selenium;
        private readonly IMangaRepository _mangaRepository;
        private readonly IGenereRepository _genereRepository;
        private readonly ILogger<MangaScraperService> _logger;
        private readonly Settings _settings;

        public MangaScraperService(ISeleniumService selenium, IMangaRepository mangaRepostiry, IGenereRepository genereRepository, ILogger<MangaScraperService> logger, Settings settings)
        {
            _selenium = selenium;
            _mangaRepository = mangaRepostiry;
            _genereRepository = genereRepository;
            _logger = logger;
            _settings = settings;
        }

        private ChromeOptions CreateChromeOptions(Settings settings)
        {
            ChromeOptions options = new ChromeOptions();
            try
            {
                //Aggiungo l'estensione AdBlock
                options.AddExtensions(settings.adBlockExtensionLocation);

                //Rimuovo il pop-up per impostare il motore di ricerca predefinito
                options.AddArgument("--disable-search-engine-choice-screen");

                //Per qualche motivo ricevo in console errori sul certificato anche se la connessione è sicura.
                options.AcceptInsecureCertificates = true;

                return options;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: estensione non trovata nella posizione: {settings.adBlockExtensionLocation}. {ex}", settings.adBlockExtensionLocation, ex);
                throw;
            }
        }

        private IWebDriver CreateDriverWithExtensions(ChromeOptions opt)
        {
            try
            {
                return _selenium.CreateChromeDriver(opt);
            }
            catch(Exception ex) 
            {
                _logger.LogError("ERRORE: creazione ChromeDriver con le opzioni desiderate non riuscita. {ex}", ex);
                throw;
            }
        }

        //Metodo per ottenere le pagine dell'archivio. Per ogni pagina dell'archivio troviamo 16 manga.
        private IEnumerable<string> GetArchivePageUrls(Settings settings)
        {
            try
            {
                List<string> urls = new List<string>();

                int nPages = settings.nPagine;
                string baseUrl = settings.baseUrl;
                string param = settings.nextPageParamArchive;

                for (int i = 0; i < nPages; i++)
                {
                    if(i > 0)
                    {
                        urls.Add($"{baseUrl}{param}{i + 1}");
                    }
                    else
                    {
                        urls.Add(baseUrl);
                    }
                }

                return urls;
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: costruzione degli url delle pagine dell'archivio non andata a buon fine. {ex}", ex);
                throw;
            }
        }

        private IWebDriver StartMangaScraping()
        {
            try
            {
                ChromeOptions options = CreateChromeOptions(_settings);

                return CreateDriverWithExtensions(options);
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nell'inizializzazione dello scraping: {ex}", ex);
                throw;
            }
        }

        private IEnumerable<Manga> CreateManga(IWebDriver driver)
        {
            try
            {
                //Ottengo gli url di tutte le pagine dell'archivio che mi interessano
                List<string> archivePageUrls = GetArchivePageUrls(_settings).ToList();

                List<Manga> mangas = new List<Manga>();

                //Navigo in ogni pagina dell'archivio
                foreach (string url in archivePageUrls)
                {
                    _selenium.GoToUrl(url, driver);

                    //Costruisco i manga con le informazioni trovate nella pagina.
                    //I manga contengono anche gli url alla pagina del singolo manga
                    mangas = GetInfoManga(driver).ToList();
                }

                return mangas;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE nell'ottenimento degli url delle pagine dei singoli manga. {ex}", ex);
                throw;
            }
        }

        //Metodo da utilizzare quando si è all'interno della pagina del singolo manga per estrarre gli url di ogni capitolo
        private IEnumerable<string> GetChapterPagesOfManga(IWebDriver driver)
        {
            try
            {
                //Estraggo il div con i capitoli. Devo eseguire questo passaggio perchè altri <a> con classe "chap" (i quali non sono capitoli del manga che sto visualizzando) sono presenti nella pagina.
                IWebElement divChapers = _selenium.FindElementByClassName("chapters-wrapper", driver);

                //La classe degli <a> con gli href desiderati è: "chap". DA INSERIRE QUESTA CSS CLASS NEL FILE appsettings.json??????
                List<IWebElement> aElements = _selenium.FindElementsByClassName("chap", divChapers).ToList();

                List<string> urlsCapitoli = new List<string>();

                foreach(IWebElement aElement in aElements)
                {
                    urlsCapitoli.Add(_selenium.GetAttribute("href", aElement));
                }

                return urlsCapitoli;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE nell'ottenimento degli url dei capitoli. {ex}", ex);
                throw;
            }
        }

        //Metodo che viene utilizzato quando ci troviamo nella pagina dell'archivio per prendere le informazioni dei manga presenti in quella pagina
        private IEnumerable<Manga> GetInfoManga(IWebDriver driver)
        {
            List<Manga> mangas = new List<Manga>();

            //Metodo tramite il quale otteniamo i <div> contenenti le informazioni dei manga sulla pagina dell'archivio
            IEnumerable<IWebElement> mangaGrid = _selenium.FindElementsByClassName("content", driver);

            //Ottengo la lista dei generi dal db dato che mi servirà dopo ed è piu afficiente tenerla in memoria che effettuare query ad ogni iterazione
            //Probabilmente non è efficiente farlo qui -> BISOGNA FARLO NEL METODO OPERATE() E PASSARE LA LISTA DEI GENERI A QUESTO METODO
            List<Genere> genereList = _genereRepository.GetGeneres().ToList();

            foreach(IWebElement mangaInfo in mangaGrid)
            {
                //Ottengo il Nome del manga
                IWebElement a_Name = _selenium.FindElementByClassName("manga-title", mangaInfo);
                string nome = a_Name.Text;

                //Ottengo l'Url del manga
                string url = _selenium.GetAttribute("href", a_Name);

                //Ottengo il tipo del manga
                IWebElement div_Tipo = _selenium.FindElementByClassName("genre", mangaInfo);
                string tipo = _selenium.FindElementByTagName("a", div_Tipo).Text;

                //Ottengo lo Stato del manga
                IWebElement div_Stato = _selenium.FindElementByClassName("status", mangaInfo);
                string stato = _selenium.FindElementByTagName("a", div_Stato).Text;

                //Ottengo l'Autore del manga
                IWebElement div_Autore = _selenium.FindElementByClassName("author", mangaInfo);
                string autore = _selenium.FindElementByTagName("a", div_Autore).Text;

                //Ottengo l'Artista del manga
                IWebElement div_Artista = _selenium.FindElementByClassName("artist", mangaInfo);
                string artista = _selenium.FindElementByTagName("a", div_Artista).Text;

                //Ottengo la lista dei generi del manga -> DA SISTEMARE PER VIA DEL CAMBIO STRUTTURA DB
                IWebElement div_Generi = _selenium.FindElementByClassName("genres", mangaInfo);

                //Ottengo la trama del manga
                string trama = _selenium.FindElementByClassName("story", mangaInfo).Text;

                List<IWebElement> generis = _selenium.FindElementsByTagName("a", div_Generi).ToList();

                List<Genere> generi = new List<Genere>();

                //Estraggo ogni nome del genere del manga per cercarlo nella lista dei generi salvati su db
                foreach(IWebElement element in generis)
                {
                    //Rimuovo gli spazi dal nome del genere dato che nell'enum non ci sono spazi
                    string genereName = Regex.Replace(element.Text, @"[\s-]", "");

                    generi.Add(genereList.Find(g => g.NameId == genereName) ?? throw new Exception());
                }

                Manga manga = new Manga(nome, url, tipo, stato, autore, artista, trama);

                //Imposto la lista dei generi trovati come proprietà di navigazione del manga
                manga.Generi = generi;

                mangas.Add(manga);
            }

            return mangas;
        }

        //Metodo per Download Immagini capitoli
        private async Task DownloadImgs(HttpClient downloader, string fileToDownload, string imgPath)
        {
            try
            {
                HttpResponseMessage response = await downloader.GetAsync(fileToDownload);
                response.EnsureSuccessStatusCode();

                using (var stream = await response.Content.ReadAsStreamAsync())

                using (var fileStream = new FileStream(imgPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }

                _logger.LogInformation("SUCCESSO - Download immagine: {fileToDownload}", fileToDownload);
            }
            catch (Exception ex)
            {
                _logger.LogError("Errore nel download dell'immagine {fileToDownload}. {ex}", fileToDownload, ex);
                throw;
            }
        }

        //Metodo che estrae le immagini del capitolo, le scarica e le salva su disco rinominandolo
        private async Task GetManhwaChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga, HttpClient downloader)
        {
            List<string> urlsCapitoliString = urlsCapitoli.ToList();

            Regex pattern = new Regex("[/]");
            string newMangaName = pattern.Replace(manga.Nome, "-");

            //Dato che i Manhwa non hanno volumi ma solamente capitoli, creo un unico volume fittizio tramite il quale gestisco la relazione con i capitoli
            Volume volume = new Volume("Volume Fittizio", manga.Id);
            manga.Volumi.Add(volume);

            for (int i = 0; i < urlsCapitoliString.Count; i++)
            {
                //Navigo sulla pagina del capitolo
                _selenium.GoToUrl(urlsCapitoliString[i], driver);

                //Creo la cartella del manga se non esiste già
                DirectoryInfo mangaFolder = Directory.CreateDirectory(_settings.folderForImages + $"//{newMangaName}");

                //Creo la cartella del capitolo se non esiste già
                DirectoryInfo chapterFolder = Directory.CreateDirectory($"{mangaFolder.FullName}" + $"//Capitolo {i + 1}");

                //Estraggo il <div> con id="page", il quale contiene tutte le immagini del capitolo manhwa
                IWebElement divImgs = _selenium.FindElementById("page", driver);

                //Estraggo la lista di tutte le immagini del capitolo
                List<IWebElement> imgs = _selenium.FindElementsByTagName("img", divImgs).ToList();

                Capitolo capitolo = new Capitolo($"{i + 1}", volume.Id);

                volume.Capitoli.Add(capitolo);

                for (int j = 0; j < imgs.Count; j++)
                {
                    string urlImgToDownload = _selenium.GetAttribute("src", imgs[j]);

                    //Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroCapitolo + numeroImmagineCapitolo.jpg
                    await DownloadImgs(downloader, urlImgToDownload, chapterFolder.FullName + $"\\{newMangaName}" + $"Capitolo{i + 1}_img{j + 1}.jpg");

                    capitolo.ImgPositions.Add(new ImagePosition(chapterFolder.FullName + $"\\{newMangaName}" + $"Capitolo{i + 1}_img{j + 1}.jpg", capitolo.Id));
                }
            }
        }

        private async Task GetMangaChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga, HttpClient downloader)
        {
            List<string> urlsCapitoliString = urlsCapitoli.ToList();

            Regex pattern = new Regex("[/]");
            string newMangaName = pattern.Replace(manga.Nome, "-");

            for(int i = 0; i < urlsCapitoliString.Count; i++)
            {
                //Navigo sulla pagina del capitolo
                _selenium.GoToUrl(urlsCapitoliString[i], driver);

                IWebElement volumeDropDown = _selenium.FindElementByClassName("volume", driver);

                //Ottengo il numero del volume dalla <select> con le <option>
                string numVolume = _selenium.FindElementsByTagName("option", volumeDropDown)
                    .Where(o => o.Selected == true)
                    .Last().Text;

                //Creo la cartella del manga se non esiste già
                DirectoryInfo mangaFolder = Directory.CreateDirectory(_settings.folderForImages + $"//{newMangaName}");

                //Creo la cartella del volume se non esiste già
                DirectoryInfo volumeFolder = Directory.CreateDirectory($"{mangaFolder.FullName}" + $"//{numVolume}");

                //Creo la cartella del capitolo se non esiste già
                DirectoryInfo chapterFolder = Directory.CreateDirectory($"{volumeFolder.FullName}" + $"//Capitolo {i + 1}");

                //Ottengo il numero delle pagine del capitolo dalla dropdown nella pagina
                IWebElement pagineDropDown = _selenium.FindElementByClassName("page", driver);

                int nPagine = _selenium.FindElementsByTagName("option", pagineDropDown).Count();

                //Creo gli oggetti per assegnare gli id in maniera opportuna
                Volume volume = new Volume(numVolume, manga.Id);

                Capitolo capitolo = new Capitolo($"{i + 1}", volume.Id);

                //Aggiungo gli oggetti creati nelle proprietà di navigazione
                manga.Volumi.Add(volume);
                volume.Capitoli.Add(capitolo);

                //Estraggo il <div> contenente l'immagine perchè esiste un altro elemento con la stessa classe dell'immagine
                IWebElement imgDiv = _selenium.FindElementById("page", driver);

                for (int j = 0; j < nPagine; j++)
                {
                    //Estraggo l'immagine
                    string urlImg = _selenium.GetAttributeOfElementByClassName("img-fluid", "src", imgDiv);

                    //Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroVolume + numeroCapitolo + numeroImmagineCapitolo.jpg
                    await DownloadImgs(downloader, urlImg, chapterFolder.FullName + $"\\{newMangaName}" + $"Capitolo{i + 1}_img{j + 1}.jpg");

                    capitolo.ImgPositions.Add(new ImagePosition(chapterFolder.FullName + $"\\{newMangaName}" + $"Capitolo{i + 1}_img{j + 1}.jpg", capitolo.Id));

                    _selenium.FindElementByClassName("page-next", driver).Click();
                }
            }
        }

        private async Task GetOneShotChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga, HttpClient downloader)
        {
            List<string> urlsCapitoliString = urlsCapitoli.ToList();

            Regex pattern = new Regex("[/]");
            string newMangaName = pattern.Replace(manga.Nome, "-");

            //Dato che i OneShot non hanno nè volumi nè capitoli ma solamente immagini, creo un unico volume e un unico capitolo fittizi tramite i quali gestisco la relazione
            Volume volume = new Volume("Volume Fittizio", manga.Id);

            Capitolo capitolo = new Capitolo("Capitolo Fittizio", volume.Id);

            manga.Volumi.Add(volume);
            volume.Capitoli.Add(capitolo);

            for (int i = 0; i < urlsCapitoliString.Count; i++)
            {
                //Navigo sulla pagina del capitolo
                _selenium.GoToUrl(urlsCapitoliString[i], driver);

                //Creo la cartella del manga se non esiste già
                DirectoryInfo mangaFolder = Directory.CreateDirectory(_settings.folderForImages + $"//{newMangaName}");

                //Ottengo il numero delle pagine del capitolo dalla dropdown nella pagina
                IWebElement pagineDropDown = _selenium.FindElementByClassName("page", driver);

                int nPagine = _selenium.FindElementsByTagName("option", pagineDropDown).Count();

                //Estraggo il <div> contenente l'immagine perchè esiste un altro elemento con la stessa classe dell'immagine
                IWebElement imgDiv = _selenium.FindElementById("page", driver);

                //Estraggo il pulsante da cliccare per cambiare pagina dinamicamente
                IWebElement nextPage = _selenium.FindElementByClassName("page-next", driver);

                for (int j = 0; j < nPagine; j++)
                {
                    //Estraggo l'immagine
                    string urlImg = _selenium.GetAttributeOfElementByClassName("img-fluid", "src", imgDiv);

                    //Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroImmagineCapitolo.jpg
                    await DownloadImgs(downloader, urlImg, mangaFolder.FullName + $"\\{newMangaName}_img{j + 1}.png");

                    capitolo.ImgPositions.Add(new ImagePosition(mangaFolder.FullName + $"\\{newMangaName}_img{j + 1}.png", capitolo.Id));

                    nextPage.Click();
                }
            }
        }

        public async Task Operate()
        {
            IWebDriver driver = StartMangaScraping();

            //Vado sulla home page di goolge e aspetto 5 secondi, così da dare il tempo per l'installazione dell'estensione dell'AdBlock
            _selenium.GoToUrl("https://www.google.it/", driver);

            Thread.Sleep(6 *  1000);

            //Dopo l'installazione dell'AdBlock si apre una nuova Tab. Switcho con il driver su quella tab e la chiudo
            driver.SwitchTo().Window(driver.WindowHandles.Last()).Close();
            driver.SwitchTo().Window(driver.WindowHandles.First());

            driver.Manage().Window.Maximize();

            //Estraggo le info dei manga da ogni pagina dell'archivio e creo gli oggetti di dominio
            List<Manga> mangas = CreateManga(driver).ToList();

            HttpClient downloader = new HttpClient();

            foreach(Manga manga in mangas)
            {
                //Navigo su ogni pagina del manga
                _selenium.GoToUrl(manga.Url, driver);

                //La lista dei capitoli è in ordine decrescente.
                List<string> urlsCapitoli = GetChapterPagesOfManga(driver).ToList();

                urlsCapitoli.Reverse();

                switch (manga.Tipo)
                {
                    case "Manga": await GetMangaChapterImgs(urlsCapitoli, driver, manga, downloader);
                        break;

                    case "Manhua": await GetMangaChapterImgs(urlsCapitoli, driver, manga, downloader);
                        break;

                    case "Oneshot": await GetOneShotChapterImgs(urlsCapitoli, driver, manga, downloader);
                        break;

                    case "Manhwa": await GetManhwaChapterImgs(urlsCapitoli, driver, manga, downloader);
                        break;

                    default: break;
                }
            }

            //Probabilmente transaction inutile essendoci solamente un SaveChanges()
            using (var transaction = await _mangaRepository.BeginTransactionAsync())
            {
                try
                {
                    _mangaRepository.InsertRangeAsyncManga(mangas);
                    await _mangaRepository.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("ERRORE: riscontrati problemi nell'esecuzione degli insert tramite mangaRepository. {ex}", ex);
                    await transaction.RollbackAsync();
                }
            }

            _selenium.QuitDriver(driver);
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
