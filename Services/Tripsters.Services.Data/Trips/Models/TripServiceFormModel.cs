namespace Tripsters.Services.Data.Trips.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Common;

    public class TripServiceFormModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.TripSecurity.NameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(GlobalConstants.TripSecurity.DestinationMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.DestinationMinLength)]
        public string From { get; set; }

        [Required]
        [StringLength(GlobalConstants.TripSecurity.DestinationMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.DestinationMinLength)]
        public string To { get; set; }

        public DateTime StartDate { get; set; }

        [Range(GlobalConstants.TripSecurity.AvailableSeatsMinRange, GlobalConstants.TripSecurity.AvailableSeatsMaxRange)]
        public int AvailableSeats { get; set; }

        [StringLength(GlobalConstants.TripSecurity.DescriptionMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.DescriptionMinLength)]
        public string Description { get; set; }
    }
}
