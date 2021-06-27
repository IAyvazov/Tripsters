namespace Tripsters.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Town : BaseDeletableModel<string>
    {
        public Town()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Landmarks = new HashSet<Landmark>();
            this.FromTrips = new HashSet<Trip>();
            this.ToTrips = new HashSet<Trip>();
        }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Trip> FromTrips { get; set; }

        public ICollection<Trip> ToTrips { get; set; }

        public ICollection<Landmark> Landmarks { get; set; }
    }
}