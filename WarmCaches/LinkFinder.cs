using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EastCoast.BrokenLinkTests
{
    public class LinkFinder
    {
        private ChromeDriver Driver { get; set; }

        private string BaseUrl { get; set; }

        public LinkFinder(ChromeDriver driver, string baseUrl)
        {
            Driver = driver;
            BaseUrl = baseUrl;
        }

        private List<string> GetLinkUrlsForPage(string url)
        {
            Driver.GoToUrl(url);
            IReadOnlyList<IWebElement> links = Driver.FindElements(By.TagName("a"));
            return links.Select(link => link.GetDomProperty("href")).ToList();
        }

        public List<string> GetInternalLinkUrlsForPage(string url)
        {
            return GetLinkUrlsForPage(url)
                .Where(link => link.IsInternalWebPageLink(BaseUrl))
                .Select(link => link.CleanAnchorFromUrl().CleanVariablesFromUrl())
                .Distinct()
                .ToList();
        }
    }
}
