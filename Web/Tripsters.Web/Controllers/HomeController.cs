namespace Tripsters.Web.Controllers
{
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Services.Data.Trips;
    using Tripsters.Web.ViewModels;

    public class HomeController : BaseController
    {
        private readonly ITripsService tripsService;

        public HomeController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public IActionResult Index()
        {
            return this.View(this.tripsService.AllCategories());
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
