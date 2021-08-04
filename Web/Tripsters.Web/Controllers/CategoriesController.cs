namespace Tripsters.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Services.Data.Trips;
    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Trips;
    using Tripsters.Web.ViewModels.Users;

    public class CategoriesController : Controller
    {
        private readonly ITripsService tripsService;

        public CategoriesController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public IActionResult AllCategoryTrips(TripsListingModel model)
        {
            var trips = new TripsListingModel
            {
                Trips = ConvertFromServiceToViewModel(this.tripsService.GetAllTripsByCategoryId(model.CategoryId, model.CurrentPage, model.TripsPerPage)),
            };
            return this.View(trips);
        }

        private static ICollection<TripsViewModel> ConvertFromServiceToViewModel(ICollection<TripServiceModel> trips)
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
