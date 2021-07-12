namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public string Text { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
