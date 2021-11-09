using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;

namespace EastCoast.BrokenLinkTests
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            List<string> homepageLinks;
            string baseUrl = args[0];
            Console.WriteLine($"Checking pages liked from {baseUrl}");
            using (var driver = new ChromeDriver())
            {
                homepageLinks = new LinkFinder(driver, baseUrl).GetInternalLinkUrlsForPage(baseUrl);
            }

            if (!homepageLinks.Any())
            {
                throw new Exception($"No links found on homepage: {baseUrl}");
            }

            Console.WriteLine($"{homepageLinks.Count} page links found for homepage:");
            foreach (string link in homepageLinks)
            {
                Console.WriteLine($"\t{link}");
            }

            ConcurrentBag<string> brokenLinks = new ConcurrentBag<string>();
            ConcurrentBag<string> errorLinks = new ConcurrentBag<string>();
            int retryMax = 3;

            Parallel.ForEach(homepageLinks,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                (link) =>
                {
                    int retryCount = 0;
                    while (retryCount < retryMax)
                    {
                        try
                        {
                            using var driver = new ChromeDriver();
                            driver.GoToUrl(link);

                            if (driver.Title.Contains("Sorry This Page Does Not Exist | LNER"))
                            {
                                brokenLinks.Add(link);
                            }
                            break;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            retryCount++;
                            if (retryCount == retryMax)
                            {
                                errorLinks.Add(link);
                            }
                        }
                    }
                });

            var successCount = homepageLinks.Count - brokenLinks.Count - errorLinks.Count;
            Console.WriteLine($"Total pages successfully loaded: {successCount}");

            if (brokenLinks.Any())
            {
                Console.WriteLine($"WARNING: {brokenLinks.Count} broken page links found:");
                foreach (string link in brokenLinks)
                {
                    Console.WriteLine($"\t{link}");
                }
            }

            if (errorLinks.Any())
            {
                Console.WriteLine($"ERROR: {errorLinks.Count} broken page links found:");
                foreach (string link in errorLinks)
                {
                    Console.WriteLine($"\t{link}");
                }
                throw new Exception($"ERROR: {errorLinks.Count} links errored when tested");
            }
        }
    }
}
