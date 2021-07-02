namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class UserTrip : BaseDeletableModel<string>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
