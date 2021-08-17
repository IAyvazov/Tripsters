namespace Tripsters.Tests.Routing
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Tripsters.Web.Controllers;
    using Tripsters.Web.ViewModels.Trips;
    using Tripsters.Services.Data.Users.Models;

    public class TripsControllerTests
    {
        [Fact]
        public void TripsAllRouteShouldMatchCorrectController()
        => MyRouting
         .Configuration()
         .ShouldMap("/Trips/All")
         .To<TripsController>(c => c.All(With.Any<TripsListingModel>()));

        [Fact]
        public void RecentTripsRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/RecentTrips")
          .To<TripsController>(c => c.RecentTrips(With.Any<UserProfileServiceModel>()));


        [Fact]
        public void AddRouteShouldMatchCorrectController()
          => MyRouting
           .Configuration()
           .ShouldMap("/Trips/Add")
           .To<TripsController>(c => c.Add(With.Any<TripsInputFormModel>()));


        [Fact]
        public void CommentRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Comment")
          .To<TripsController>(c => c.Comment(With.Any<string>()));

        [Fact]
        public void JoinRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Join")
          .To<TripsController>(c => c.Join(With.Any<string>(), With.Any<string>()));

        [Fact]
        public void DetailsRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Details")
          .To<TripsController>(c => c.Details(With.Any<string>()));

        [Fact]
        public void CreatorRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Creator")
          .To<TripsController>(c => c.Creator(With.Any<string>(), With.Any<string>(), With.Any<string>()));

        [Fact]
        public void MembersRouteShouldMatchCorrectController()
        => MyRouting
         .Configuration()
         .ShouldMap("/Trips/Members")
         .To<TripsController>(c => c.Members(With.Any<string>()));

        [Fact]
        public void MyTripsRouteShouldMatchCorrectController()
        => MyRouting
         .Configuration()
         .ShouldMap("/Trips/MyTrips")
         .To<TripsController>(c => c.MyTrips(With.Any<TripsListingModel>()));

        [Fact]
        public void DeleteRouteShouldMatchCorrectController()
        => MyRouting
         .Configuration()
         .ShouldMap("/Trips/Delete")
         .To<TripsController>(c => c.Delete(With.Any<string>()));

        [Fact]
        public void EditRouteShouldMatchCorrectController()
        => MyRouting
         .Configuration()
         .ShouldMap("/Trips/Edit")
         .To<TripsController>(c => c.Edit(With.Any<string>()));

        [Fact]
        public void UpcomingRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Upcoming")
          .To<TripsController>(c => c.Upcoming(With.Any<TripsListingModel>()));

        [Fact]
        public void PastRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Past")
          .To<TripsController>(c => c.Past(With.Any<TripsListingModel>()));

        [Fact]
        public void LikeRouteShouldMatchCorrectController()
         => MyRouting
          .Configuration()
          .ShouldMap("/Trips/Like")
          .To<TripsController>(c => c.Like(With.Any<string>()));
    }
}
