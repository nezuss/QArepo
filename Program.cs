using System.Text;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Text;

namespace NUnit_SEQA
{
    [TestFixture]
    public class Tests
    {
        private IWebDriver driver;
        private string baseURL;
        private StringBuilder verificationErrors;
        private string path;

        [SetUp]
        public void Setup()
        {
            driver = new FirefoxDriver();
            baseURL = "https://the-internet.herokuapp.com/dynamic_controls";
            verificationErrors = new StringBuilder();
            path = "C:/Users/AkuToR/source/repos/NUnit_SEQA/";
        }

        [Test]
        public async void Test1()
        {
            driver.Navigate().GoToUrl(baseURL);

            ClassicAssert.AreEqual("The Internet", driver.Title);

            Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path + "scr_start.png");

            //? Checking Remove/add
            var checkboxRemoveButton = driver.FindElement(By.XPath("//button[contains(text(),'Remove')]"));
            checkboxRemoveButton.Click();

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.FindElement(By.Id("message")).Text == "It's gone!");

            screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path + "scr_checkbox_removed.png");

            Assert.Throws<NoSuchElementException>(() => driver.FindElement(By.Id("checkbox")));

            var checkboxAddButton = driver.FindElement(By.XPath("//button[contains(text(),'Add')]"));
            checkboxAddButton.Click();

            wait.Until(d => d.FindElement(By.Id("message")).Text == "It's back!");

            screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path + "scr_checkbox_added.png");

            //? Checking Enable/Disable
            var enableButton = driver.FindElement(By.XPath("//button[contains(text(),'Enable')]"));
            enableButton.Click();

            wait.Until(d => d.FindElement(By.Id("message")).Text == "It's enabled!");

            screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path + "scr_input_enabled.png");

            var disableButton = driver.FindElement(By.XPath("//button[contains(text(),'Disable')]"));
            disableButton.Click();

            Assert.Throws<NoSuchElementException>(() => driver.FindElement(By.Id("input-example")).FindElement(By.XPath("//input[@disabled]")));

            wait.Until(d => d.FindElement(By.Id("message")).Text == "It's disabled!");

            screenshot = ((ITakesScreenshot)driver).GetScreenshot();
            screenshot.SaveAsFile(path + "scr_input_disabled.png");
        }

        [TearDown]
        public void Teardown()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            ClassicAssert.AreEqual("", verificationErrors.ToString());
        }

        static public void Main(string[] args)
        {
            Tests tests = new Tests();
            Console.WriteLine("Starting tests...");
            tests.Setup();
            tests.Test1();
            tests.Teardown();
        }
    }
}
