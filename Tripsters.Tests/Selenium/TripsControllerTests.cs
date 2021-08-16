namespace Tripsters.Tests.Controllers
{
    using OpenQA.Selenium.Chrome;
    using Xunit;

    using Tripsters.Tests.Data;
    using Tripsters.Web;

    public class TripsControllerTests
    {
        [Fact]
        public void TripsAllShouldReturnH1ElementWithAllTripsText()
        {
            // Arrange
            var serverFactory = new SeleniumServerFactory<Startup>();

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AcceptInsecureCertificates = true;

            var webDriver = new ChromeDriver(options);

            webDriver.Navigate().GoToUrl(serverFactory.RootUri + "/Trips/All");

            // Act
            var element = webDriver.FindElementByTagName("h1");

            // Assert
            Assert.NotNull(element);
            Assert.Equal("All Trips", element.Text);
        }

        [Fact]
        public void TripsAllShouldReturnThreeTripCards()
        {
            // Arrange
            var serverFactory = new SeleniumServerFactory<Startup>();

            var options = new ChromeOptions();
            options.AddArgument("--headless");
            options.AcceptInsecureCertificates = true;

            var webDriver = new ChromeDriver(options);

            webDriver.Navigate().GoToUrl(serverFactory.RootUri + "/Trips/All");
            // Act
            var element = webDriver.FindElementsByClassName("card");

            // Assert
            Assert.NotNull(element);
            Assert.Equal(3, element.Count);
        }

       
    }
}
