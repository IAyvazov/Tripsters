namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsUpcomingListingViewModel
    {
        public IEnumerable<TripsViewModel> TodayTrips { get; set; }

        public IEnumerable<TripsViewModel> TomorrowTrips { get; set; }
    }
}
