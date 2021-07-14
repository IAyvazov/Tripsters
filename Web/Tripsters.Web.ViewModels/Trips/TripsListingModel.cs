namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsListingModel
    {
        public int TripsPerPage { get; } = 3;

        public ICollection<TripsViewModel> Trips { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int TotalTrips { get; set; }
    }
}
