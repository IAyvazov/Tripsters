namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    using Tripsters.Web.ViewModels.Users;

    public class TripsListingModel
    {
        public ICollection<TripsViewModel> Trips { get; set; }

    }
}
