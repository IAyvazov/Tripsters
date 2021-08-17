namespace Tripsters.Tests.Routing
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Tripsters.Services.Data.Users.Models;
    using Tripsters.Web.Controllers;

    public class UsersControllerTests
    {
        [Fact]
        public void ProfileRouteShouldMatchCorrectController()
       => MyRouting
        .Configuration()
        .ShouldMap("/Users/Profile")
        .To<UsersController>(c => c.Profile(With.Any<UserProfileServiceModel>()));

        [Fact]
        public void EditRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Users/Edit")
          .To<UsersController>(c => c.Edit(With.Any<UserProfileServiceModel>()));
    }
}
