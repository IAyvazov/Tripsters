namespace Tripsters.Web.Controllers
{
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

            return this.View();
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
        public IActionResult AddTrip(TripsInputFormModel trips)
        {
            if (this.User == null)
            {
                return this.Unauthorized();
            }

            this.tripsService.Add(trips);

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
