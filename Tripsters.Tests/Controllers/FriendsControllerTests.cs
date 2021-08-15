namespace Tripsters.Tests.Controllers
{
    using System.Linq;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Tripsters.Services.Data.Users.Models;
    using Tripsters.Web.Controllers;
    using Tripsters.Web;

    using static Data.Users;
    using Tripsters.Tests.Data;
    using OpenQA.Selenium.Chrome;

    public class FriendsControllerTests
    {
        //[Fact]
        //public void MyFriends()
        //{
        //    var serverFactory = new SeleniumServerFactory<Startup>();

        //    var options = new ChromeOptions();
        //    options.AddArgument("--headless");
        //    options.AcceptInsecureCertificates = true;

        //    var webDriver = new ChromeDriver(options);

        //    webDriver.Navigate().GoToUrl(serverFactory.RootUri + "/Friends/MyFriends");
        //    var element = webDriver.FindElementByClassName("card-body");

        //    Assert.Null(element);
        //}
    }
}
