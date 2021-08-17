namespace Tripsters.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Common;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Notifications;
    using Tripsters.Services.Data.Trips;
    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Trips;
    using Tripsters.Web.ViewModels.Users;

    using static Tripsters.Common.GlobalConstants;

    public class TripsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INotificationsService notificationsService;
        private readonly ITripsService tripsService;
        private readonly IUsersService usersService;
        private readonly IBadgesService badgesService;

        public TripsController(
            UserManager<ApplicationUser> userManager,
            INotificationsService notificationsService,
            ITripsService tripsService,
            IUsersService usersService,
            IBadgesService badgesService)
        {
            this.tripsService = tripsService;
            this.usersService = usersService;
            this.badgesService = badgesService;
            this.notificationsService = notificationsService;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult RecentTrips(UserProfileServiceModel userModel)
        {
            var user = this.usersService
                .GetUserProfileById(userModel.UserId, userModel.CurrentPage, userModel.PhotosPerPage);

            user.RecentTrips = this.tripsService
                .RecentTrips(userModel.UserId, userModel.CurrentPage, userModel.PhotosPerPage);

            return this.View(user);
        }

        public IActionResult All(TripsListingModel model)
        {
            var tripCount = this.tripsService.GetAllTripsCount();

            var trips = this.tripsService.GetAllTrips(model.CurrentPage, model.TripsPerPage);

            if (model.SearchTerm != null)
            {
                trips = trips.Where(t =>
                          t.CreatorName.ToLower().Contains(model.SearchTerm.ToLower()) ||
                          t.From.ToLower().Contains(model.SearchTerm.ToLower()) ||
                          t.To.ToLower().Contains(model.SearchTerm.ToLower()) ||
                          t.Name.ToLower().Contains(model.SearchTerm.ToLower()))
                    .ToList();

                tripCount = trips.Count();
            }

            var tripsModel = this.ConvertFromServiceToViewModel(trips);

            model = new TripsListingModel { Trips = tripsModel, CurrentPage = model.CurrentPage, TotalTrips = tripCount };

            return this.View(model);
        }

        [Authorize]
        public IActionResult Add()
        {
            var categories = new HashSet<TripCategoryVIewModel>();

            foreach (var category in this.tripsService.AllCategories())
            {
                categories.Add(new TripCategoryVIewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }

            return this.View(new TripsInputFormModel
            {
                Categories = categories,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(TripsInputFormModel trip)
        {
            if (!this.ModelState.IsValid || trip.StartDate.DayOfYear < DateTime.Now.DayOfYear)
            {
                return this.View(trip);
            }

            var userId = this.userManager.GetUserId(this.User);
            await this.tripsService.AddTrip(
                new TripServiceFormModel
                {
                    Id = trip.Id,
                    Name = trip.Name,
                    AvailableSeats = trip.AvailableSeats,
                    From = trip.From,
                    To = trip.To,
                    StartDate = trip.StartDate,
                    Description = trip.Description,
                    CategoryId = trip.CategoryId,
                },
                userId);

            this.TempData[GlobalMessageKey] = "You trip was added and is awaiting for approval!";

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public IActionResult Comment(string tripId)
        {
            var userId = this.userManager.GetUserId(this.User);

            var comments = this.tripsService.GetAllTripComments(tripId);
            var tripName = this.tripsService.GetTripById(tripId, userId).Name;
            var user = this.usersService.GetUser(userId);

            var userProfilePictureUrl = user.Photos
                .Where(p => p.IsProfilePicture)
                .Select(p => p.Url)
                .FirstOrDefault();

            var model = new CommentListingVIewModel
            {
                Comments = comments,
                TripId = tripId,
                TripName = tripName,
                UserProfilePictureUrl = userProfilePictureUrl,
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(CommentFormModel commentData)
        {
            var currUserId = this.userManager.GetUserId(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.Comment), new { commentData.TripId });
            }

            await this.tripsService.AddComment(currUserId, commentData.TripId, commentData.Text);

            var userTrip = this.tripsService
                .GetTripById(commentData.TripId, currUserId);

            await this.notificationsService.Notifie(currUserId, userTrip.CreatorId, Notifications.CommentText + userTrip.Name);

            return this.Redirect($"/Trips/Comment?tripId={commentData.TripId}");
        }

        [Authorize]
        public async Task<IActionResult> Join(string tripId, string userId)
        {
            var currUserId = this.userManager.GetUserId(this.User);

            await this.tripsService.JoinTrip(tripId, userId);
            var trip = this.tripsService.GetTripById(tripId, userId);

            await this.notificationsService.Notifie(currUserId, trip.CreatorId, Notifications.JoinText + trip.Name);

            this.TempData[GlobalMessageKey] = "You have successfully joined this trip!";

            return this.View(trip);
        }

        [Authorize]
        public IActionResult Details(string tripId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var trip = this.tripsService.GetTripById(tripId, userId);

            this.ViewBag.IsAvailableSeats = trip.AvailableSeats > 0;

            return this.View(trip);
        }

        [Authorize]
        public IActionResult Creator(string creatorId, string userId, string currTripId)
        {
            var user = this.usersService.GetUserById(creatorId, userId, currTripId);

            return this.View(user);
        }

        [Authorize]
        public IActionResult Members(string tripId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [Authorize]
        public IActionResult MyTrips(TripsListingModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var trips = this.ConvertFromServiceToViewModel(this.tripsService.GetAllUserTrips(userId, model.CurrentPage, model.TripsPerPage));

            var tripCount = this.tripsService.GetAllUserTripsCount(userId);

            model = new TripsListingModel { Trips = trips, CurrentPage = model.CurrentPage, TotalTrips = tripCount };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string tripId)
        {
            await this.tripsService.Delete(tripId);

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.TempData[GlobalMessageKey] = "This trip was successfully deleted!";

                return this.Redirect("/Administration/Trips/Index");
            }

            this.TempData[GlobalMessageKey] = "Your trip was successfully deleted!";

            return this.Redirect("/Trips/MyTrips");
        }

        [Authorize]
        public IActionResult Edit(string tripId)
        {
            var userId = this.userManager.GetUserId(this.User);
            var trip = this.tripsService.GetTripById(tripId, userId);

            DateTime.TryParseExact(trip.StartDate, "G", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate);

            var model = new TripsInputFormModel
            {
                Id = trip.Id,
                Name = trip.Name,
                AvailableSeats = trip.AvailableSeats,
                StartDate = startDate,
                From = trip.From,
                To = trip.To,
                Description = trip.Description,
            };

            var categories = new HashSet<TripCategoryVIewModel>();

            foreach (var category in this.tripsService.AllCategories())
            {
                categories.Add(new TripCategoryVIewModel
                {
                    Id = category.Id,
                    Name = category.Name,
                });
            }

            model.Categories = categories;

            return this.View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(string tripId, TripsInputFormModel trip)
        {
            if (!this.ModelState.IsValid || trip.StartDate.DayOfYear < DateTime.Now.DayOfYear)
            {
                return this.View(trip);
            }

            await this.tripsService.EditTrip(
                tripId,
                new TripServiceFormModel
                {
                    Id = tripId,
                    Name = trip.Name,
                    AvailableSeats = trip.AvailableSeats,
                    From = trip.From,
                    To = trip.To,
                    StartDate = trip.StartDate,
                    Description = trip.Description,
                    CategoryId = trip.CategoryId,
                });

            if (this.User.IsInRole(GlobalConstants.AdministratorRoleName))
            {
                this.TempData[GlobalMessageKey] = "This trip was successfully edited!";

                return this.Redirect("/Administration/Trips/Index");
            }

            this.TempData[GlobalMessageKey] = "Your trip was successfully edited!";

            return this.Redirect("/Trips/MyTrips");
        }

        [Authorize]
        public IActionResult Upcoming(TripsListingModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var todayTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetUpcommingTodayTrips(userId));
            var tomorrowTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetUpcommingTomorrowTrips(userId));

            var upcomingTripsModel = new TripsUpcomingListingViewModel { TodayTrips = todayTrips, TomorrowTrips = tomorrowTrips };

            var viewModel = new TripsListingModel { UpcomingTrips = upcomingTripsModel, CurrentPage = model.CurrentPage, TotalTrips = upcomingTripsModel.TodayTrips.Count() + upcomingTripsModel.TomorrowTrips.Count() };

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Past(TripsListingModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var pastTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetPastTrips(userId, model.CurrentPage, model.TripsPerPage));
            var badges = this.badgesService.GetAllBadges();

            List<BadgeViewModel> badgesModel = new();

            foreach (var badge in badges)
            {
                badgesModel.Add(new BadgeViewModel
                {
                    Id = badge.Id,
                    Name = badge.Name,
                });
            }

            model = new TripsListingModel
            { Trips = pastTrips, CurrentPage = model.CurrentPage, TotalTrips = pastTrips.Count(), Badges = badgesModel };

            return this.View(model);
        }

        [Authorize]
        public async Task<IActionResult> Like(string tripId)
        {
            var currUserId = this.userManager.GetUserId(this.User);
            await this.tripsService.LikeTrip(tripId, currUserId);

            var trip = this.tripsService.GetTripById(tripId, currUserId);

            await this.notificationsService.Notifie(currUserId, trip.CreatorId, Notifications.LikeText + trip.Name);

            return this.RedirectToAction(nameof(this.Past));
        }

        private IEnumerable<TripsViewModel> ConvertFromServiceToViewModel(IEnumerable<TripServiceModel> trips)
        => trips.Select(t => new TripsViewModel
        {
            Id = t.Id,
            Name = t.Name,
            From = t.From,
            To = t.To,
            CreatorId = t.CreatorId,
            CreatorName = t.CreatorName,
            AvailableSeats = t.AvailableSeats,
            CurrentUserId = t.CurrentUserId,
            StartDate = t.StartDate,
            Description = t.Description,
            Comments = t.Comments,
            CategoryName = t.CategoryName,
            Likes = t.Likes,
            Members = t.Members
            .Select(m => new UserViewModel
            {
                UserName = m.UserName,
                Age = m.Age,
                Id = m.Id,
                CurrentTripId = t.Id,
                Badges = m.Badges
                .Select(b => new BadgeViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                }).ToList(),
            }).ToList(),
        }).ToList();
    }
}
