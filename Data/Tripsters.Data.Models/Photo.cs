namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class Photo : BaseDeletableModel<int>
    {
        public string Url { get; set; }

        public bool IsProfilePicture { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
