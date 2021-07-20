namespace Tripsters.Services.Data.Trips.Models
{
    using System.Collections.Generic;

    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Users.Models;

    public class TripServiceModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public int AvailableSeats { get; set; }

        public string Description { get; set; }

        public string CreatorName { get; set; }

        public string CreatorId { get; set; }

        public string StartDate { get; set; }

        public string CurrentUserId { get; set; }

        public int Likes { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<UserServiceModel> Members { get; set; }
    }
}
