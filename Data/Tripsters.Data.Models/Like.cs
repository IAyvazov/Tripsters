namespace Tripsters.Data.Models
{
    public class Like
    {
        public int Id { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int? PhotoId { get; set; }

        public Photo Photo { get; set; }
    }
}
