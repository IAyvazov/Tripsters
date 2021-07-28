namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class Destination : BaseDeletableModel<int>
    {
        public string From { get; set; }

        public string To { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }
    }
}