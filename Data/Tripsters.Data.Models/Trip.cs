namespace Tripsters.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Trip : BaseDeletableModel<int>
    {
        public Trip()
        {
            this.Participants = new HashSet<ApplicationUser>();
        }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int AvailableSeats { get; set; }

        public int FromTownId { get; set; }

        public Town FromTown { get; set; }

        public int ToTownId { get; set; }

        public Town ToTown { get; set; }

        public ICollection<ApplicationUser> Participants { get; set; }

        public bool IsAvailable
            => this.AvailableSeats != 0;
    }
}
