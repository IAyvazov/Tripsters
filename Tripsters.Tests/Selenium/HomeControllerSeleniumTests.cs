namespace Tripsters.Tests.Controllers
{
    using OpenQA.Selenium.Chrome;
    using Xunit;

    using Tripsters.Tests.Data;
    using Tripsters.Web;

    public class HomeControllerSeleniumTests
    {
        [Fact]
        public void IndexShouldVisualizeFiveButtons()
        {
            // Arrange
            var serverFactory = new SeleniumServerFactory<Startup>();

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AcceptInsecureCertificates = true;

            var webDriver = new ChromeDriver(options);

            webDriver.Navigate().GoToUrl(serverFactory.RootUri + "/");
            // Act
            var buttons = webDriver.FindElementsByTagName("a");

            // Assert
            Assert.NotNull(buttons);
            Assert.Contains(buttons, b => b.Text.Contains("Make Trip"));
            Assert.Contains(buttons, b => b.Text.Contains("All Trips"));
            Assert.Contains(buttons, b => b.Text.Contains("Sky"));
            Assert.Contains(buttons, b => b.Text.Contains("Water"));
            Assert.Contains(buttons, b => b.Text.Contains("Mountain"));
        }
    }
}
