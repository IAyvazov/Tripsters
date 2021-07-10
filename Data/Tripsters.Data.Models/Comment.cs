namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class Comment : BaseDeletableModel<int>
    {
        public string TripId { get; set; }

        public Trip Trip { get; set; }
    }
}
