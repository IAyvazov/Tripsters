﻿namespace Tripsters.Data.Models
{
    using System.Collections.Generic;

    public class Like
    {
        public int Count { get; set; }

        public string TripId { get; set; }

        public Trip Trip { get; set; }

        public string UserId { get; set; }

        public ICollection<ApplicationUser> User { get; set; }

        // public int? PhotoId { get; set; }

        // public Photo Photo { get; set; }
    }
}
