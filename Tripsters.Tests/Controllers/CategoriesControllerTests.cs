namespace Tripsters.Tests.Controllers
{
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    using Tripsters.Web.Controllers;
    using Tripsters.Web.ViewModels.Trips;

    using static Data.Trips;

    public class CategoriesControllerTests
    {
        [Fact]
        public void AllTripsByCategoryShouldReturnViewWithCorectData()
            => MyMvc
                .Pipeline()
                     .ShouldMap("/Categories/AllTripsByCategory")
                     .To<CategoriesController>(c => c.AllTripsByCategory(new TripsListingModel { }))
                     .Which(controller => controller
                        .WithData(TripsByCategory))
                    .ShouldReturn()
                    .View(view => view
                        .WithModelOfType<TripsListingModel>());
    }
}
