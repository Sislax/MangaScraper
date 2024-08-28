﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace MangaScraper.Interfaces
{
    public interface ISeleniumService
    {
        /*
         * Cercando di seguire i SOLID PRINCIPLES mi trovo a dover creare quest'astrazione che però mi crea dubbi sulla correttezza in quanto i metodi da implementare in ogni classe sono molti
         * LEGGERE FILE README PER MAGGIORI SPIEGAZIONI SUI DUBBI
         */
        public IWebDriver CreateChromeDriver();
        public IWebDriver CreateChromeDriver(ChromeOptions options);
        public void QuitDriver(IWebDriver driver);
        public void GoToUrl(string url, IWebDriver driver);
        public IWebElement FindElementByClassName(string className, IWebDriver driver);
        public IWebElement FindElementByClassName(string className, IWebElement element);
        public IEnumerable<IWebElement> FindElementsByClassName(string className, IWebDriver driver);
        public IEnumerable<IWebElement> FindElementsByClassName(string className, IWebElement element);
        public IWebElement FindElementByTagName(string tagName, IWebElement element);
        public IWebElement FindElementByTagName(string tagName, IWebDriver driver);
        public IEnumerable<IWebElement> FindElementsByTagName(string tagName, IWebElement element);
        public IEnumerable<IWebElement> FindElementsByTagName(string tagName, IWebDriver driver);
        public IWebElement FindElementById(string id, IWebDriver driver);
        public IWebElement FindElementById(string id, IWebElement element);
        public IEnumerable<IWebElement> FindElementsById(string id, IWebDriver driver);
        public IEnumerable<IWebElement> FindElementsById(string id, IWebElement element);
        public string GetAttribute(string attribute, IWebElement element);
        public string GetAttributeOfElementByClassName(string className, string attribute, IWebDriver driver);
        public string GetAttributeOfElementByClassName(string className, string attribute, IWebElement element);
        public void Click(IWebElement element);
    }
}