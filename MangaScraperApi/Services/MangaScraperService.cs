using MangaScraperApi.Interfaces.ServiceInterfaces;
using MangaScraperApi.Interfaces.RepoInterfaces;
using MangaScraperApi.Models.Domain;
using MangaScraperApi.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text.RegularExpressions;
using MangaScraperApi.Models;
using OpenQA.Selenium.Support.UI;

namespace MangaScraperApi.Services
{
    public class MangaScraperService : IMangaScraperService
    {
        private readonly ISeleniumService _selenium;
        private readonly IAppRepository _appRepository;
        private readonly ILogger<MangaScraperService> _logger;
        private readonly HttpClient _client;
        private readonly Settings _settings;

        public MangaScraperService(ISeleniumService selenium, IAppRepository appRepository, ILogger<MangaScraperService> logger, HttpClient client, Settings settings)
        {
            _selenium = selenium;
            _appRepository = appRepository;
            _logger = logger;
            _client = client;
            _settings = settings;
        }

        private IWebDriver StartMangaScraping()
        {
            try
            {
                ChromeOptions options = _selenium.CreateChromeOptions(_settings);

                return _selenium.CreateChromeDriver(options);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE - Problemi nell'inizializzazione dello scraping:\n{ex}", ex);
                throw;
            }
        }

        //Metodo per ottenere le pagine dell'archivio. Per ogni pagina dell'archivio troviamo 16 manga.
        private IEnumerable<string> GetArchivePageUrls(Settings settings, int nPages)
        {
            try
            {
                List<string> urls = new List<string>();

                string baseUrl = settings.BaseUrl;

                for (int i = 0; i < nPages; i++)
                {
                    if (i > 0)
                    {
                        urls.Add($"{baseUrl}?page={i + 1}");
                    }
                    else
                    {
                        urls.Add(baseUrl);
                    }
                }

                return urls;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE - Costruzione degli url delle pagine dell'archivio non andata a buon fine:\n{ex}", ex);
                throw;
            }
        }

        //Metodo che crea la lista dei manga con tutte le info
        private async Task<List<Manga>> CreateManga(IWebDriver driver, int nPages, List<Genere> genereList)
        {
            try
            {
                //Ottengo gli url di tutte le pagine dell'archivio che mi interessano
                List<string> archivePageUrls = GetArchivePageUrls(_settings, nPages).ToList();

                List<Manga> mangas = new List<Manga>();

                //Navigo in ogni pagina dell'archivio
                foreach (string url in archivePageUrls)
                {
                    _selenium.GoToUrl(url, driver);

                    //Costruisco i manga con le informazioni trovate nella pagina.
                    //I manga contengono anche gli url alla pagina del singolo manga
                    mangas = await GetInfoManga(driver, genereList);
                }

                return mangas;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE - Problemi nell'ottenimento degli url delle pagine dei singoli manga:\n{ex}", ex);
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

                foreach (IWebElement aElement in aElements)
                {
                    urlsCapitoli.Add(_selenium.GetAttribute("href", aElement));
                }

                return urlsCapitoli;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE - Problemi nell'ottenimento degli url dei capitoli:\n{ex}", ex);
                throw;
            }
        }

        //Metodo che viene utilizzato quando ci troviamo nella pagina dell'archivio per prendere le informazioni dei manga presenti in quella pagina
        private async Task<List<Manga>> GetInfoManga(IWebDriver driver, List<Genere> genereList)
        {
            try
            {
                List<Manga> mangas = new List<Manga>();

                //Metodo tramite il quale otteniamo i <div> contenenti le informazioni dei manga sulla pagina dell'archivio
                IEnumerable<IWebElement> mangaGrid = _selenium.FindElementsByClassName("entry", driver);

                //Regex per "/" nel nome del manga
                Regex pattern = new Regex("[/]");

                foreach (IWebElement mangaInfo in mangaGrid)
                {
                    //Ottengo il Nome del manga
                    IWebElement a_Name = _selenium.FindElementByClassName("manga-title", mangaInfo);
                    string nome = a_Name.Text;

                    //Ottengo l'Url del manga
                    string url = _selenium.GetAttribute("href", a_Name);

                    //Ottengo il percorso dell'immagine di copertina
                    IWebElement a_Copertina = _selenium.FindElementByClassName("thumb", mangaInfo);
                    IWebElement imgCopertina = _selenium.FindElementByTagName("img", a_Copertina);
                    string copertinaUrl = _selenium.GetAttribute("src", imgCopertina);

                    string newMangaName = pattern.Replace(nome, "-");

                    //Creo la cartella delle copertine se non esiste già
                    DirectoryInfo copertineFolder = Directory.CreateDirectory(_settings.FolderForImages + $"//Copertine");

                    //Scarico la copertina del manga e la salvo nell'apposita cartella
                    await DownloadImgs(copertinaUrl, copertineFolder.FullName + $"\\{newMangaName}.jpg");

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

                    //Ottengo la lista dei generi del manga
                    IWebElement div_Generi = _selenium.FindElementByClassName("genres", mangaInfo);

                    //Ottengo la trama del manga
                    string trama = _selenium.FindElementByClassName("story", mangaInfo).Text;

                    List<IWebElement> generis = _selenium.FindElementsByTagName("a", div_Generi).ToList();

                    List<Genere> generi = new List<Genere>();

                    //Estraggo ogni nome del genere del manga per cercarlo nella lista dei generi salvati su db
                    foreach (IWebElement element in generis)
                    {
                        //Rimuovo gli spazi dal nome del genere dato che nell'enum non devono esserci spazi
                        string genereName = Regex.Replace(element.Text, @"[\s-]", "");

                        generi.Add(genereList.Find(g => g.NameId == genereName) ?? throw new Exception());
                    }

                    Manga manga = new Manga(nome, copertineFolder.FullName + $"\\{newMangaName}", url, tipo, stato, autore, artista, trama);

                    //Imposto la lista dei generi trovati come proprietà di navigazione del manga
                    manga.Generi = generi;

                    mangas.Add(manga);
                }

                return mangas;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: Impossibile ottenere le info dei manga dalla pagina dell'archivio. {ex}", ex);
                throw;
            }
        }

        //Metodo per Download Immagini capitoli
        private async Task DownloadImgs(string fileToDownload, string imgPath)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync(fileToDownload, HttpCompletionOption.ResponseHeadersRead); // Ottimizzazione per stream di grandi dimensioni
                response.EnsureSuccessStatusCode();

                // Aprire il file stream e scrivere i dati ricevuti
                await using (var stream = await response.Content.ReadAsStreamAsync())
                await using (var fileStream = new FileStream(imgPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true)) // Buffer ottimizzato a 8 KB
                {
                    await stream.CopyToAsync(fileStream);
                }

                _logger.LogInformation("SUCCESSO - Download completato:\n{fileToDownload}", fileToDownload);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE - Problemi nel download dell'immagine:\n{fileToDownload}.\n{ex}", fileToDownload, ex);
                throw;
            }
        }

        //Metodo che estrae le immagini del capitolo, le scarica e le salva su disco rinominandolo
        private async Task GetManhwaChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga)
        {
            try
            {
                List<string> urlsCapitoliString = urlsCapitoli.ToList();

                //Dato che "/" non è ammesso nel nome delle cartelle lo sostuisco con "-" 
                Regex pattern = new Regex("[/]");
                string newMangaName = pattern.Replace(manga.Nome, "-");

                //Dato che i Manhwa non hanno volumi ma solamente capitoli, creo un unico volume fittizio tramite il quale gestisco la relazione con i capitoli
                Volume volume = new Volume("Volume Fittizio", manga.Id);
                manga.Volumi.Add(volume);

                for (int i = 0; i < urlsCapitoliString.Count; i++)
                {
                    //Navigo sulla pagina del capitolo
                    _selenium.GoToUrl(urlsCapitoliString[i], driver);

                    //Ottengo il numero del capitolo (di solito va in ordine quindi potrei utilizzare la i, ma ci sono casi in cui è tipo Capitolo 3.5)
                    IWebElement capitoloDropdown = _selenium.FindElementByClassName("chapter", driver);

                    string numCapitolo = _selenium.FindElementsByTagName("option", capitoloDropdown)
                            .Where(o => o.Selected == true)
                            .Last().Text;

                    //Creo la cartella del manga se non esiste già
                    DirectoryInfo mangaFolder = Directory.CreateDirectory(_settings.FolderForImages + $"//{newMangaName}");

                    //Creo la cartella del capitolo se non esiste già
                    DirectoryInfo chapterFolder = Directory.CreateDirectory($"{mangaFolder.FullName}" + $"//{numCapitolo}");

                    //Estraggo il <div> con id="page", il quale contiene tutte le immagini del capitolo manhwa
                    IWebElement divImgs = _selenium.FindElementById("page", driver);

                    //Estraggo la lista di tutte le immagini del capitolo
                    List<IWebElement> imgs = _selenium.FindElementsByTagName("img", divImgs).ToList();

                    Capitolo capitolo = new Capitolo($"{numCapitolo}", volume.Id);

                    volume.Capitoli.Add(capitolo);

                    for (int j = 0; j < imgs.Count; j++)
                    {
                        string urlImgToDownload = _selenium.GetAttribute("src", imgs[j]);

                        //Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroCapitolo + numeroImmagineCapitolo.jpg
                        await DownloadImgs(urlImgToDownload, chapterFolder.FullName + $"\\{newMangaName}" + $"{numCapitolo}_img{j + 1}.jpg");

                        capitolo.ImgPositions.Add(new ImagePosition(chapterFolder.FullName + $"\\{newMangaName}" + $"{numCapitolo}_img{j + 1}.jpg", capitolo.Id));

                        Thread.Sleep(500);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: Impossibile ottenere le immagini del manwha: {manga.Nome}. {ex}", manga.Nome, ex);
                throw;
            }
        }

        private async Task GetMangaChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga)
        {
            try
            {
                List<string> urlsCapitoliString = urlsCapitoli.ToList();

                Regex pattern = new Regex("[/]");
                string newMangaName = pattern.Replace(manga.Nome, "-");

                for (int i = 0; i < urlsCapitoliString.Count; i++)
                {
                    //Navigo sulla pagina del capitolo
                    _selenium.GoToUrl(urlsCapitoliString[i], driver);

                    IWebElement volumeDropDown;

                    string numVolume = "";

                    //Dato che c'è stato un caso in cui un manga non contenesse volumi, gestisco questo particolare caso in questo try-catch
                    try
                    {
                        volumeDropDown = _selenium.FindElementByClassName("volume", driver);

                        //Ottengo il numero del volume dalla <select> con le <option>
                        numVolume = _selenium.FindElementsByTagName("option", volumeDropDown)
                            .Where(o => o.Selected == true)
                            .Last().Text;
                    }
                    catch (Exception ex)
                    {
                        numVolume = "Volume Fittizio";
                        _logger.LogWarning("WARNING - La dropdown dei volumi non è disponibile nell'attuale manga. Verrà assegnato un volume fittizio.\n{ex}", ex);
                    }

                    //Ottengo il numero del capitolo (di solito va in ordine quindi potrei utilizzare la i, ma ci sono casi in cui è tipo Capitolo 3.5)
                    IWebElement capitoloDropdown = _selenium.FindElementByClassName("chapter", driver);

                    string numCapitolo = _selenium.FindElementsByTagName("option", capitoloDropdown)
                            .Where(o => o.Selected == true)
                            .Last().Text;

                    //Creo la cartella del manga se non esiste già
                    DirectoryInfo mangaFolder = Directory.CreateDirectory(_settings.FolderForImages + $"//{newMangaName}");

                    //Creo la cartella del volume se non esiste già
                    DirectoryInfo volumeFolder = Directory.CreateDirectory($"{mangaFolder.FullName}" + $"//{numVolume}");

                    //Creo la cartella del capitolo se non esiste già
                    DirectoryInfo chapterFolder = Directory.CreateDirectory($"{volumeFolder.FullName}" + $"//{numCapitolo}");

                    //Ottengo il numero delle pagine del capitolo dalla dropdown nella pagina
                    IWebElement pagineDropDown = _selenium.FindElementByClassName("page", driver);

                    int nPagine = _selenium.FindElementsByTagName("option", pagineDropDown).Count();

                    Volume? existingVolume = manga.Volumi.FirstOrDefault(v => v.NumVolume == numVolume);

                    Capitolo capitolo = null!;

                    //Se il volume è già presente utilizzo quello, altrimenti ne creo uno nuovo
                    if (existingVolume != null)
                    {
                        capitolo = new Capitolo($"{numCapitolo}", existingVolume.Id);

                        //Aggiungo gli oggetti creati nelle proprietà di navigazione
                        manga.Volumi.Add(existingVolume);
                        existingVolume.Capitoli.Add(capitolo);
                    }
                    else
                    {
                        Volume volume = new Volume(numVolume, manga.Id);

                        capitolo = new Capitolo($"{numCapitolo}", volume.Id);

                        //Aggiungo gli oggetti creati nelle proprietà di navigazione
                        manga.Volumi.Add(volume);
                        volume.Capitoli.Add(capitolo);
                    }

                    //Estraggo il <div> contenente l'immagine perchè esiste un altro elemento con la stessa classe dell'immagine
                    IWebElement imgDiv = _selenium.FindElementById("page", driver);

                    for (int j = 0; j < nPagine; j++)
                    {

                        //Estraggo l'immagine
                        string urlImg = _selenium.GetAttributeOfElementByClassName("img-fluid", "src", imgDiv);

                        //Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroVolume + numeroCapitolo + numeroImmagineCapitolo.jpg
                        await DownloadImgs(urlImg, chapterFolder.FullName + $"\\{newMangaName}" + $"{numCapitolo}_img{j + 1}.jpg");

                        capitolo.ImgPositions.Add(new ImagePosition(chapterFolder.FullName + $"\\{newMangaName}" + $"{numCapitolo}_img{j + 1}.jpg", capitolo.Id));

                        //Aspetto due secondi prima di cliccare
                        Thread.Sleep(500);

                        _selenium.FindElementByClassName("page-next", driver).Click();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: Impossibile ottenere le immagini del manga: {manga.Nome}. {ex}", manga.Nome, ex);
                throw;
            }
        }

        private async Task GetOneShotChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga)
        {
            try
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
                    DirectoryInfo mangaFolder = Directory.CreateDirectory(_settings.FolderForImages + $"//{newMangaName}");

                    //Ottengo il numero delle pagine del capitolo dalla dropdown nella pagina
                    IWebElement pagineDropDown = _selenium.FindElementByClassName("page", driver);

                    int nPagine = _selenium.FindElementsByTagName("option", pagineDropDown).Count();

                    //Estraggo il <div> contenente l'immagine perchè esiste un altro elemento con la stessa classe dell'immagine
                    IWebElement imgDiv = _selenium.FindElementById("page", driver);

                    //Estraggo il pulsante da cliccare per cambiare pagina dinamicamente
                    //IWebElement nextPage = _selenium.FindElementByClassName("page-next", driver);

                    for (int j = 0; j < nPagine; j++)
                    {

                        //Estraggo l'immagine
                        string urlImg = _selenium.GetAttributeOfElementByClassName("img-fluid", "src", imgDiv);

                        //Salvo l'immagine nell'apposita cartella, chiamando il file con: nomeDelManga + numeroImmagineCapitolo.jpg
                        await DownloadImgs(urlImg, mangaFolder.FullName + $"\\{newMangaName}_img{j + 1}.png");

                        capitolo.ImgPositions.Add(new ImagePosition(mangaFolder.FullName + $"\\{newMangaName}_img{j + 1}.png", capitolo.Id));

                        //Aspetto due secondi prima di cliccare
                        Thread.Sleep(500);

                        _selenium.FindElementByClassName("page-next", driver).Click();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: Impossibile ottenere le immagini del oneshot: {manga.Nome}. {ex}", manga.Nome, ex);
                throw;
            }

        }

        private async Task GetChapterImgs(IEnumerable<string> urlsCapitoli, IWebDriver driver, Manga manga)
        {
            switch (manga.Tipo)
            {
                case "Manga":
                    await GetMangaChapterImgs(urlsCapitoli, driver, manga);
                    break;

                case "Manhua":
                    await GetMangaChapterImgs(urlsCapitoli, driver, manga);
                    break;

                case "Oneshot":
                    await GetOneShotChapterImgs(urlsCapitoli, driver, manga);
                    break;

                case "Manhwa":
                    await GetManhwaChapterImgs(urlsCapitoli, driver, manga);
                    break;

                default: break;
            }
        }

        //Metodo per riavviare il driver. Anche se le performance ne risentono, libero la memoria e prevengo il blocco del driver dal sito.
        private IWebDriver RestartDriver(IWebDriver driver, WebDriverWait wait)
        {
            try
            {
                _selenium.QuitDriver(driver);

                IWebDriver newDriver = StartMangaScraping();

                wait = CreateWaitWithNewDriver(newDriver);

                newDriver.Manage().Cookies.DeleteAllCookies();

                //Vado sulla home page di goolge e aspetto 5 secondi, così da dare il tempo per l'installazione dell'estensione dell'AdBlock
                _selenium.GoToUrl("https://www.google.it/", newDriver);

                wait.Until(d => d.WindowHandles.Count > 1);

                //Dopo l'installazione dell'AdBlock si apre una nuova Tab. Switcho con il driver su quella tab e la chiudo
                newDriver.SwitchTo().Window(newDriver.WindowHandles.Last()).Close();
                newDriver.SwitchTo().Window(newDriver.WindowHandles.First());

                newDriver.Manage().Window.Maximize();

                return newDriver;
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: Impossibile uscire dal driver. {ex}", ex);
                throw;
            }
        }

        //Metodo che aggiorna il WebDriverWait quando viene creato il nuovo driver
        private WebDriverWait CreateWaitWithNewDriver(IWebDriver driver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));

                return wait;
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: Impossibile creare il nuovo WebDriverWait con il driver passato. {ex}", ex);
                throw;
            }
        }

        public async Task Update(int nPages)
        {
            try
            {
                IEnumerable<MangaToCompare> mangaToCompareList = await _appRepository.GetCapitoliCountForEachMangaAsync();

                IWebDriver driver = StartMangaScraping();

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));

                //Vado sulla home page di goolge e aspetto per l'installazione dell'estensione dell'AdBlock
                _selenium.GoToUrl("https://www.google.it/", driver);

                wait.Until(d => d.WindowHandles.Count > 1);

                //Dopo l'installazione dell'AdBlock si apre una nuova Tab. Switcho con il driver su quella tab e la chiudo
                driver.SwitchTo().Window(driver.WindowHandles.Last()).Close();
                driver.SwitchTo().Window(driver.WindowHandles.First());

                driver.Manage().Window.Maximize();

                //Ottengo la lista dei generi salvati su db
                List<Genere> genereList = (List<Genere>)await _appRepository.GetGeneresAsync();

                //Estraggo le info dei manga da ogni pagina dell'archivio e creo gli oggetti di dominio
                //Faccio quest'operazione perchè nel caso un manga non venga trovato sul db lo devrò aggiungere, quindi sacrifico delle prestazione per mantenere un po' più chiaro il codice
                //Perchè anche se il manga è già su db devo comunque estrarre delle info per controllare che su db ci siano tutti i capitoli. Ciò che faccio è solamente estrarre delle info in più
                List<Manga> mangas = await CreateManga(driver, nPages, genereList);

                bool dbEmpty = !mangaToCompareList.Any();

                foreach (Manga manga in mangas)
                {
                    //Navigo sulla pagina di ogni manga
                    _selenium.GoToUrl(manga.Url, driver);

                    //La lista dei capitoli è in ordine decrescente.
                    List<string> urlsCapitoli = GetChapterPagesOfManga(driver).ToList();

                    urlsCapitoli.Reverse();

                    //Se in mangaToCompareList non è presente nulla vuol dire che il db è vuoto. Quindi Aggiungo tutti i manga e salto ciò che viene effettuato dopo.
                    if (dbEmpty)
                    {
                        await GetChapterImgs(urlsCapitoli, driver, manga);

                        //Aggiorno il database dopo ogni manga in modo che se qualcosa va storto durante la lunga esecuzione dello scraping, ho salvato tutti i dati fino a quel momento, anche se le performance ne risentono
                        await _appRepository.AddMangaAsync(manga);
                        await _appRepository.SaveChangesAsync();

                        continue;
                    }

                    //Ottengo il mangaToCompare relativo allo specifico manga che sto analizzando sul sito
                    //Il nome non è la chiave primaria ma comunque non ci saranno mai due manga con lo stesso ed identico nome
                    MangaToCompare? mangaToCompare = mangaToCompareList.FirstOrDefault(m => m.Nome == manga.Nome);

                    if (mangaToCompare == null)
                    {
                        //Se mangaToCompare == null vuol dire che il manga che sto analizzando non è presente sul db quindi è da aggiungere interamente, seguo lo stesso metodo utilizzato in Operate()
                        await GetChapterImgs(urlsCapitoli, driver, manga);

                        //Aggiorno il database dopo ogni manga in modo che se qualcosa va storto durante la lunga esecuzione dello scraping, ho salvato tutti i dati fino a quel momento, anche se le performance ne risentono
                        await _appRepository.AddMangaAsync(manga);
                        await _appRepository.SaveChangesAsync();
                    }
                    else
                    {
                        //Se mangaToCompare != null vuol dire che il manga è già presente sul db, quindi controllo se il numero dei capitoli corrisponde
                        if (urlsCapitoli.Count > mangaToCompare.NCapitoli)
                        {
                            //Sostituisco il manga analizzato dal ciclo, con il corrispettivo presente sul db e modifico quello. L'Id è DbGenerated quindi non ho modo di usare l'Id per l'update prima del salvataggio sul db
                            //Se passo il manga nell'update() viene aggiunto come fosse uno nuovo perchè non c'è una comparazione di Id per controllare se è già presente
                            Manga existingManga = await _appRepository.GetMangaByNameAsync(manga.Nome);

                            //Nel caso il numero di capitoli sul sito sono maggiori di quelli salvati sul database, creo la lista degli url con i capitoli mancanti
                            List<string> newUrlsList = new List<string>();

                            for (int i = mangaToCompare.NCapitoli; i < urlsCapitoli.Count; i++)
                            {
                                newUrlsList.Add(urlsCapitoli[i]);
                            }

                            //Passo il manga presente su db
                            await GetChapterImgs(newUrlsList, driver, existingManga);

                            _appRepository.UpdateManga(existingManga);
                            await _appRepository.SaveChangesAsync();
                        }
                    }
                }

                driver.Quit();
                driver.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE - Problemi nell'esecuzione degli update tramite mangaRepository:\n{ex}", ex);
                throw;
            }
        }
    }
}
