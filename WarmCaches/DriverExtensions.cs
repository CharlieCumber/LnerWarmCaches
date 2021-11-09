using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using static SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace EastCoast.BrokenLinkTests
{
    public static class DriverExtensions
    {
        private static By UsernameInputLocator => By.Id("LoginControl_UserName");
        private static By PasswordInputLocator => By.Id("LoginControl_Password");
        private static By SubmitButtonLocator => By.XPath(XPathHelpers.ElementWithValue("input", "Log in"));

        public static void GoToUrl(this ChromeDriver driver, string url)
        {
            driver.Url = url;
            if (driver.IsEpiserverLoginScreen())
            {
                driver.PerformEpiserverLogin();
            }
        }

        private static bool IsEpiserverLoginScreen(this ChromeDriver driver)
        {
            return driver.Title == "Login" && driver.Url.Contains("/Util/login.aspx");
        }

        private static void PerformEpiserverLogin(this ChromeDriver driver)
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(40)).Until(ElementToBeClickable(UsernameInputLocator));
            driver.FindElement(UsernameInputLocator).SendKeys("<insert username here>");
            driver.FindElement(PasswordInputLocator).SendKeys("<insert password here>");
            driver.FindElement(SubmitButtonLocator).Click();
        }
    }
}
