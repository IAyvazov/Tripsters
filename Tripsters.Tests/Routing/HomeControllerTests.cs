namespace Tripsters.Tests.Routing
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Tripsters.Web.Controllers;

    public class HomeControllerTests
    {
        [Fact]
        public void CustomRouteShouldMatchCorrectController()
        => MyRouting
            .Configuration()
            .ShouldMap("/")
            .To<HomeController>(c => c.Index());
    }
}
