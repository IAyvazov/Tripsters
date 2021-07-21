﻿namespace Tripsters.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Trips;
    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Services.Data.Users;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITripsService tripsService;
        private readonly IUsersService usersService;
        private readonly IBadgesService badgesService;

        public TripsController(
            ITripsService tripsService,
            IUsersService usersService,
            IBadgesService badgesService,
            UserManager<ApplicationUser> userManager)
        {
            this.tripsService = tripsService;
            this.usersService = usersService;
            this.badgesService = badgesService;
            this.userManager = userManager;
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

            var userId = this.userManager.GetUserId(this.User);
            await this.tripsService.AddTrip(trips, userId);

            return this.Redirect("/Trips/All");
        }

        [Authorize]
        public async Task<IActionResult> Comment(string tripId)
        {
            var comments = this.tripsService.GetAllTripComments(tripId);
            var userId = this.userManager.GetUserId(this.User);
            var tripName = this.tripsService.GetTripById(tripId, userId).Name;
            var user = this.usersService.GetUser(userId);
            var userProfilePictureUrl = user.Photos
                .Where(p => p.IsProfilePicture)
                .Select(p => p.Url)
                .FirstOrDefault();
            var model = new CommentListingVIewModel { Comments = comments, TripId = tripId, TripName = tripName, UserProfilePictureUrl = userProfilePictureUrl };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Comment(CommentFormModel commentData)
        {
            var userId = this.userManager.GetUserId(this.User);
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
            var userId = this.userManager.GetUserId(this.User);
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
            var userId = this.userManager.GetUserId(this.User);
            var trip = this.tripsService.GetTripById(tripId, userId);

            return this.View(trip);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditTrip(TripServiceModel trips)
        {
            await this.tripsService.EditTrip(trips);

            return this.Redirect("/Trips/MadeByMe");
        }

        public IActionResult Upcoming()
        {
            var userId = this.userManager.GetUserId(this.User);
            var todayTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetUpcommingTodayTrips(userId));
            var tomorrowTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetUpcommingTomorrowTrips(userId));
            var dayAfterTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetDayAfterTrips(userId));

            var upcomingTripsModel = new TripsUpcomingListingViewModel { TodayTrips = todayTrips, TomorrowTrips = tomorrowTrips, TheDayAfterTrips = dayAfterTrips };

            return this.View(upcomingTripsModel);
        }

        public IActionResult Past(TripsListingModel model)
        {
            var userId = this.userManager.GetUserId(this.User);
            var pastTrips = this.ConvertFromServiceToViewModel(this.tripsService.GetPastTrips(userId, model.CurrentPage, model.TripsPerPage));
            var badges = this.badgesService.GetAllBadges();
            model = new TripsListingModel { Trips = pastTrips, CurrentPage = model.CurrentPage, TotalTrips = pastTrips.Count(), Badges = badges };

            return this.View(model);
        }

        public async Task<IActionResult> Like(string tripId)
        {
            var userId = this.userManager.GetUserId(this.User);
            await this.tripsService.LikeTrip(tripId, userId);

            return this.RedirectToAction("Past");
        }

        private ICollection<TripsViewModel> ConvertFromServiceToViewModel(ICollection<TripServiceModel> trips)
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
            Likes = t.Likes,
        }).ToList();
    }
}
