namespace Tripsters.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Services.Data.Trips;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsController : BaseController
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public IActionResult All()
        {
            if (this.User == null)
            {
                return this.Unauthorized();
            }

            var trips = this.tripsService.GetAllTrips();
            var model = new TripsListingModel { Trips = trips };

            return this.View(model);
        }

        public IActionResult Add(TripsInputFormModel trips)
        {
            if (this.User == null)
            {
                return this.Unauthorized();
            }

            return this.View(trips);
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip(TripsInputFormModel trips)
        {
            if (this.User == null)
            {
                return this.Unauthorized();
            }

            await this.tripsService.AddTrip(trips);

            return this.Redirect("/Trips/All");
        }

        public IActionResult UserTrips()
        {
            if (this.User == null)
            {
                return this.Unauthorized();
            }

            return this.View();
        }
    }
}
