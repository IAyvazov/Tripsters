namespace Tripsters.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Common;
    using Tripsters.Data.Common.Models;

    public class Trip : BaseDeletableModel<string>
    {
        public Trip()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Travellers = new HashSet<UserTrip>();
            this.Likes = new HashSet<Like>();
            this.Comments = new HashSet<Comment>();
            this.Photos = new HashSet<Photo>();
        }

        [Required]
        [MaxLength(GlobalConstants.TripSecurity.NameMaxLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(2, 6)]
        public int AvailableSeats { get; set; }

        public bool IsApproved { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public Destination Destination { get; set; }

        public DateTime StartDate { get; set; }

        public ICollection<Like> Likes { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<UserTrip> Travellers { get; set; }

        public ICollection<Photo> Photos { get; set; }
    }
}
