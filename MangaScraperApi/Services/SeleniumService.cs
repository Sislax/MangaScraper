using MangaScraperApi.Interfaces.ServiceInterfaces;
using MangaScraperApi.Models.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MangaScraperApi.Services
{
    public class SeleniumService : ISeleniumService
    {
        private readonly ILogger<SeleniumService> _logger;

        public SeleniumService(ILogger<SeleniumService> logger)
        {
            _logger = logger;
        }

        public IWebDriver CreateChromeDriver()
        {
            return new ChromeDriver();
        }

        public IWebDriver CreateChromeDriver(ChromeOptions options)
        {
            try
            {
                return new ChromeDriver(options);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE durante la creazione del driver {ex}", ex);
                throw;
            }
        }

        public ChromeOptions CreateChromeOptions(Settings settings)
        {
            ChromeOptions options = new ChromeOptions();
            try
            {
                //Aggiungo l'estensione AdBlock
                options.AddExtensions(settings.AdBlockExtensionLocation);

                //Rimuovo il pop-up per impostare il motore di ricerca predefinito
                options.AddArgument("--disable-search-engine-choice-screen");
                //options.AddArgument("--headless");
                options.AddArgument("--no-sandbox");
                options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");

                //Per qualche motivo ricevo in console errori sul certificato anche se la connessione è sicura.
                //options.AcceptInsecureCertificates = true;

                return options;
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: estensione non trovata nella posizione: {settings.adBlockExtensionLocation}. {ex}", settings.AdBlockExtensionLocation, ex);
                throw;
            }
        }

        public void QuitDriver(IWebDriver driver)
        {
            try
            {
                driver.Quit();
                driver.Dispose();
                _logger.LogInformation("Chiusura del driver avvenuta con succeso");
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE durante il metodo driver.Quit() {ex}", ex);
                throw;
            }
        }

        public void GoToUrl(string url, IWebDriver driver)
        {
            try
            {
                driver.Navigate().GoToUrl(url);
            } 
            catch(Exception ex)
            { 
                _logger.LogError("ERRORE nel caricamento dell'url: {url}. {ex}", url, ex);
                throw;
            }
        }

        //Metodo per ottenere un elemento cercando nell'intera pagina
        public IWebElement FindElementByClassName(string className, IWebDriver driver)
        {
            try
            {
                return driver.FindElement(By.ClassName(className));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE, elemento con className: {className} non trovato. {ex}", className, ex);
                throw;
            }
        }

        //Overload per ottenere un elemento partendo da un elemento
        public IWebElement FindElementByClassName(string className, IWebElement element)
        {
            try
            {
                return element.FindElement(By.ClassName(className));
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: elemento con classe: {className} non trovato all'interno dell'elemento: {element}, {ex}", className, element, ex);
                throw;
            }
        }

        //Metodo per ottenere degli elementi cercando nell'intera pagina
        public IEnumerable<IWebElement> FindElementsByClassName(string className, IWebDriver driver)
        {
            try
            {
                return driver.FindElements(By.ClassName(className));
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE, elementi con className: {className} non trovati. {ex}", className, ex);
                throw;
            }
        }

        //Overload per ottenere elementi partendo da un elemento
        public IEnumerable<IWebElement> FindElementsByClassName(string className, IWebElement element)
        {
            try
            {
                return element.FindElements(By.ClassName(className));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elementi con classe: {className} non trovati all'interno dell'elemento: {element}, {ex}", className, element, ex);
                throw;
            }
        }

        public IWebElement FindElementByTagName(string tagName, IWebElement element)
        {
            try
            {
                return element.FindElement(By.TagName(tagName));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elemento con tagName: {tagName} non trovato all'interno dell'elemento: {element}, {ex}", tagName, element, ex);
                throw;
            }
        }

        public IWebElement FindElementByTagName(string tagName, IWebDriver driver)
        {
            try
            {
                return driver.FindElement(By.TagName(tagName));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elemento con tagName: {tagName} non trovato, {ex}", tagName, ex);
                throw;
            }
        }

        public IEnumerable<IWebElement> FindElementsByTagName(string tagName, IWebElement element)
        {
            try
            {
                return element.FindElements(By.TagName(tagName));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elementi con tagName: {tagName} non trovati all'interno dell'elemento: {element}, {ex}", tagName, element, ex);
                throw;
            }
        }

        public IEnumerable<IWebElement> FindElementsByTagName(string tagName, IWebDriver driver)
        {
            try
            {
                return driver.FindElements(By.TagName(tagName));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elementi con tagName: {tagName} non trovati, {ex}", tagName, ex);
                throw;
            }
        }

        public IWebElement FindElementById(string id, IWebDriver driver)
        {
            try
            {
                return driver.FindElement(By.Id(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elemento con id: {id} non trovato, {ex}", id, ex);
                throw;
            }
        }

        public IWebElement FindElementById(string id, IWebElement element)
        {
            try
            {
                return element.FindElement(By.Id(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elemento con id: {id} non trovato all'interno dell'elemento: {element}, {ex}", id, element, ex);
                throw;
            }
        }

        public IEnumerable<IWebElement> FindElementsById(string id, IWebDriver driver)
        {
            try
            {
                return driver.FindElements(By.Id(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elementi con id: {id} non trovati, {ex}", id, ex);
                throw;
            }
        }

        public IEnumerable<IWebElement> FindElementsById(string id, IWebElement element)
        {
            try
            {
                return element.FindElements(By.Id(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: elementi con id: {id} non trovati all'interno dell'elemento: {element}, {ex}", id, element, ex);
                throw;
            }
        }

        public string GetAttribute(string attribute, IWebElement element)
        {
            try
            {
                return element.GetAttribute(attribute);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE, attributo: {attribute} dell'elemento: {element} non trovato. {ex}", attribute, element, ex);
                throw;
            }
        }

        public string GetAttributeOfElementByClassName(string className, string attribute, IWebDriver driver)
        {
            try
            {
                return FindElementByClassName(className, driver).GetAttribute(attribute);
            }
            catch(Exception ex)
            {
                _logger.LogError("ERRORE: Attributo: {attribute} non trovato dall'elemento con className: {className}, {ex}", attribute, className, ex);
                throw;
            }
        }

        public string GetAttributeOfElementByClassName(string className, string attribute, IWebElement element)
        {
            try
            {
                return FindElementByClassName(className, element).GetAttribute(attribute);
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE: Attributo: {attribute} non trovato dall'elemento con className: {className}, {ex}", attribute, className, ex);
                throw;
            }
        }

        public void Click(IWebElement element)
        {
            try
            {
                element.Click();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE durante il click dell'elemento: {element}. {ex}", element, ex);
                throw;
            }
        }

        public void Refresh(IWebDriver driver)
        {
            try
            {
                driver.Navigate().Refresh();
            }
            catch (Exception ex)
            {
                _logger.LogError("ERRORE durante il refresh della pagina. {ex}", ex);
                throw;
            }
        }
    }
}
