namespace Tripsters.Test.Controllers
{
    using System.Collections.Generic;

    using MyTested.AspNetCore.Mvc;
    using FluentAssertions;
    using Xunit;

    using Tripsters.Web.Controllers;
    using Tripsters.Services.Data.Trips.Models;

    public class HomeControllerTest
    {
        [Fact]
        public void IndexShouldReturnViewWithCorrectModelAndData()
            => MyMvc
                .Pipeline()
                .ShouldMap("/")
                .To<HomeController>(c => c.Index())
            .Which()
            .ShouldReturn()
            .View(view => view
              .WithModelOfType<IEnumerable<TripCategoryServiceModel>>()
               .Passing(m => m.Should().HaveCount(3)));
    }
}
