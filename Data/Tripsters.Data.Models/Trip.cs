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
        }

        [Required]
        [MaxLength(GlobalConstants.TripSecurity.NameMaxLength)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(2, 6)]
        public int AvailableSeats { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string FromTownId { get; set; }

        public Town FromTown { get; set; }

        [Required]
        public string ToTownId { get; set; }

        public Town ToTown { get; set; }

        public DateTime StartDate { get; set; }

        public ICollection<UserTrip> Travellers { get; set; }
    }
}
