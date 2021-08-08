namespace Tripsters.Tests.Controllers
{
    using System.Collections.Generic;

    using MyTested.AspNetCore.Mvc;
    using FluentAssertions;
    using Xunit;

    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Web.Controllers;

    using static Data.Categories;
    public class HomeControllerTests
    {
        public class HomeControllerTest
        {
            [Fact]
            public void IndexShouldReturnViewWithCorrectModelAndData()
                => MyMvc
                    .Pipeline()
                     .ShouldMap("/")
                     .To<HomeController>(c => c.Index())
                     .Which(controller => controller
                        .WithData(ThreeCategories))
                    .ShouldReturn()
                    .View(view => view
                        .WithModelOfType<List<TripCategoryServiceModel>>()
                        .Passing(m => m.Should().HaveCount(3)));

            [Fact]
            public void PrivacyShouldReturnView()
            => MyMvc
                .Pipeline()
                .ShouldMap("/Home/Privacy")
                .To<HomeController>(c => c.Privacy())
                .Which()
                .ShouldReturn()
                .View();
        }
    }
}
