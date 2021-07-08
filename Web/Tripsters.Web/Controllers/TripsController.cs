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
        public IActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(TripsInputFormModel trips)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(trips);
            }

            await this.tripsService.AddTrip(trips, this.User.Identity.Name);

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public async Task<IActionResult> Join(string tripId, string userId)
        {
            await this.tripsService.JoinTrip(tripId, userId);
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        // Change to Details
        [Authorize]
        public IActionResult More(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trip = this.tripsService.GetTripById(tripId, userId);

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
        public IActionResult MyTrips()
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trips = this.tripsService.GetAllUserTrips(userId);
            var model = new TripsListingModel { Trips = trips };

            return this.View(model);
        }

        [Authorize]
        public IActionResult UserTrips()
        {
            return this.View();
        }

        [Authorize]

        public async Task<IActionResult> Delete(string tripId)
        {
            await this.tripsService.Delete(tripId);

            return this.Redirect("/Trips/MadeByMe");
        }

        [Authorize]
        public IActionResult Edit(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;

            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditTip(TripsViewModel trips)
        {
            await this.tripsService.EditTrip(trips);

            return this.Redirect("/Trips/MadeByMe");
        }

        public IActionResult Upcoming()
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var todayTrips = this.tripsService.GetUpcommingTodayTrips(userId);
            var tomorrowTrips = this.tripsService.GetUpcommingTomorrowTrips(userId);

            var upcomingTripsModel = new TripsUpcomingListingViewModel { TodayTrips = todayTrips, TomorrowTrips = tomorrowTrips };

            return this.View(upcomingTripsModel);
        }

        public IActionResult Past()
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var pastTrips = this.tripsService.GetPastTrips(userId);
            var model = new TripsListingModel { Trips = pastTrips };

            return this.View(model);
        }
    }
}
