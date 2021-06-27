namespace Tripsters.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Town : BaseDeletableModel<int>
    {
        public Town()
        {
            this.Landmarks = new HashSet<Landmark>();
            this.Trips = new HashSet<Trip>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int TripId { get; set; }

        public ICollection<Trip> Trips { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }

        public string Description { get; set; }
    }
}