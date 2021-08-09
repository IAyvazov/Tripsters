namespace Tripsters.Tests.Controllers
{
    using Tripsters.Services.Data.Users.Models;
    using Tripsters.Web.Controllers;

    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class FriendsControllerTests
    {
        [Fact]
        public void MyFriendsShouldReturnViewWithCorrectModelAndData()
        => MyMvc
            .Pipeline()
            .ShouldMap(request => request
                .WithPath("/Friends/MyFriends")
                .WithUser())
            .To<FriendsController>(c => c.MyFriends(new UserProfileServiceModel
            {
            }))
            .Which()
            .ShouldHave()
            .ActionAttributes(attributes => attributes
                .RestrictingForAuthorizedRequests())
            .AndAlso()
            .ShouldReturn()
            .View();
    }
}
