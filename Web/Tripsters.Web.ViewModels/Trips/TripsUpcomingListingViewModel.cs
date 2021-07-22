namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsUpcomingListingViewModel
    {
        public ICollection<TripsViewModel> TodayTrips { get; set; }

        public ICollection<TripsViewModel> TomorrowTrips { get; set; }
    }
}
