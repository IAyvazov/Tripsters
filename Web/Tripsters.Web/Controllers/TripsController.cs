namespace Tripsters.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Trips;
    using Tripsters.Services.Data.Users;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsController : BaseController
    {
        private readonly ITripsService tripsService;
        private readonly IUsersService usersService;
        private readonly IBadgesService badgesService;

        public TripsController(
            ITripsService tripsService,
            IUsersService usersService,
            IBadgesService badgesService)
        {
            this.tripsService = tripsService;
            this.usersService = usersService;
            this.badgesService = badgesService;
        }

        [Authorize]
        public IActionResult All(TripsListingModel model)
        {
            var tripCount = this.tripsService.GetAllTripsCount();

            var trips = this.tripsService.GetAllTrips(model.CurrentPage, model.TripsPerPage);

            if (model.SearchTerm != null)
            {
                trips = trips.Where(t =>
                          t.CreatorName.ToLower().Contains(model.SearchTerm.ToLower()) ||
                          t.FromTown.ToLower().Contains(model.SearchTerm.ToLower()) ||
                          t.ToTown.ToLower().Contains(model.SearchTerm.ToLower()) ||
                          t.Name.ToLower().Contains(model.SearchTerm.ToLower()))
                    .ToList();

                tripCount = trips.Count();
            }

            model = new TripsListingModel { Trips = trips, CurrentPage = model.CurrentPage, TotalTrips = tripCount };

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
            if (!this.ModelState.IsValid || trips.StartDate.DayOfYear < DateTime.Now.DayOfYear)
            {
                return this.View(trips);
            }

            await this.tripsService.AddTrip(trips, this.User.Identity.Name);

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public IActionResult Comment(string tripId)
        {
            var comments = this.tripsService.GetAllTripComments(tripId);
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var tripName = this.tripsService.GetTripById(tripId, userId).Name;
            var model = new CommentListingVIewModel { Comments = comments, TripId = tripId, TripName = tripName };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(CommentFormModel commentData)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            await this.tripsService.AddComment(userId, commentData.TripId, commentData.Text);

            return this.Redirect($"/Trips/Comment?tripId={commentData.TripId}");
        }

        [Authorize]
        public async Task<IActionResult> Join(string tripId, string userId)
        {
            await this.tripsService.JoinTrip(tripId, userId);
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [Authorize]
        public IActionResult Details(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trip = this.tripsService.GetTripById(tripId, userId);

            this.ViewBag.IsAvailableSeats = trip.AvailableSeats > 0;

            return this.View(trip);
        }

        public IActionResult Creator(string creatorId, string userId, string currTripId)
        {
            var user = this.usersService.GetUserById(creatorId, userId, currTripId);

            return this.View(user);
        }

        [Authorize]
        public IActionResult Members(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [Authorize]
        public IActionResult MyTrips(TripsListingModel model)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;

            var trips = this.tripsService.GetAllUserTrips(userId, model.CurrentPage, model.TripsPerPage);

            var tripCount = this.tripsService.GetAllUserTripsCount(userId);

            model = new TripsListingModel { Trips = trips, CurrentPage = model.CurrentPage, TotalTrips = tripCount };

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
        public async Task<IActionResult> EditTrip(TripsViewModel trips)
        {
            await this.tripsService.EditTrip(trips);

            return this.Redirect("/Trips/MadeByMe");
        }

        public IActionResult Upcoming()
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var todayTrips = this.tripsService.GetUpcommingTodayTrips(userId);
            var tomorrowTrips = this.tripsService.GetUpcommingTomorrowTrips(userId);
            var dayAfterTrips = this.tripsService.GetDayAfterTrips(userId);

            var upcomingTripsModel = new TripsUpcomingListingViewModel { TodayTrips = todayTrips, TomorrowTrips = tomorrowTrips, TheDayAfterTrips = dayAfterTrips };

            return this.View(upcomingTripsModel);
        }

        public IActionResult Past(TripsListingModel model)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            var pastTrips = this.tripsService.GetPastTrips(userId, model.CurrentPage, model.TripsPerPage);
            var badges = this.badgesService.GetAllBadges();
            model = new TripsListingModel { Trips = pastTrips, CurrentPage = model.CurrentPage, TotalTrips = pastTrips.Count(), Badges = badges };

            return this.View(model);
        }

        public async Task<IActionResult> Like(string tripId)
        {
            var userId = this.usersService.GetUser(this.User.Identity.Name).Id;
            await this.tripsService.LikeTrip(tripId, userId);

            return this.RedirectToAction("Past");
        }
    }
}
