namespace Tripsters.Web.ViewModels.Trips
{
    using System.Collections.Generic;

    public class CommentListingVIewModel
    {
        public ICollection<CommentViewModel> Comments { get; set; }

        public string TripId { get; set; }

        public string TripName { get; set; }
    }
}
