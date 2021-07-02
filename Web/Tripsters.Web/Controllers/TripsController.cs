namespace Tripsters.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Services.Data.Trips;
    using Tripsters.Services.Data.Users;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsController : BaseController
    {
        private readonly ITripsService tripsService;
        private readonly IUsersService usersService;

        public TripsController(ITripsService tripsService, IUsersService usersService)
        {
            this.tripsService = tripsService;
            this.usersService = usersService;
        }

        [Authorize]
        public IActionResult All()
        {
            var trips = this.tripsService.GetAllTrips();

            var model = new TripsListingModel { Trips = trips };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Add(TripsInputFormModel trips)
        {
            return this.View(trips);
        }

        [Authorize]
        public async Task<IActionResult> Join(string tripId, string userId)
        {
            await this.tripsService.JoinTrip(tripId, userId);
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [Authorize]
        public IActionResult More(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trip = this.tripsService.GetTripById(tripId, userId);

            this.ViewBag.IsUserJoined = this.tripsService.IsUserJoined(tripId, userId);
            this.ViewBag.IsAvailableSeats = trip.AvailableSeats > 0;

            return this.View(trip);
        }

        [Authorize]
        public IActionResult Members(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [Authorize]
        public IActionResult MadeByMe()
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trips = this.tripsService.GetAllUserTrips(userId);
            var model = new TripsListingModel { Trips = trips };

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTrip(TripsInputFormModel trips)
        {
            await this.tripsService.AddTrip(trips, this.User.Identity.Name);

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public IActionResult UserTrips()
        {
            return this.View();
        }
    }
}
