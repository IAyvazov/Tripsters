namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Web.ViewModels.Badges;

    public class TripsListingModel
    {
        public int TripsPerPage { get; } = 3;

        public IEnumerable<TripsViewModel> Trips { get; set; }

        public TripsUpcomingListingViewModel UpcomingTrips { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalTrips { get; set; }

        [Display(Name = "Search by text")]
        public string SearchTerm { get; set; }

        public IEnumerable<BadgeViewModel> Badges { get; set; }

        public int CategoryId { get; set; }
    }
}
