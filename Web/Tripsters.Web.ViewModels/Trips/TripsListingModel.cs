namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class TripsListingModel
    {
        public ICollection<TripsViewModel> Trips { get; set; }
    }
}
