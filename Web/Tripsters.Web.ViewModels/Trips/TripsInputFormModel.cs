namespace Tripsters.Web.ViewModels.Trips
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Common;

    public class TripsInputFormModel
    {
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(GlobalConstants.TripSecurity.NameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.NameMinLength)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "From")]
        [StringLength(GlobalConstants.TripSecurity.DestinationMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.DestinationMinLength)]
        public string From { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "To")]
        [StringLength(GlobalConstants.TripSecurity.DestinationMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.DestinationMinLength)]
        public string To { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Departure day and time")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Available Seats")]
        [Range(GlobalConstants.TripSecurity.AvailableSeatsMinRange, GlobalConstants.TripSecurity.AvailableSeatsMaxRange)]
        public int AvailableSeats { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        [StringLength(GlobalConstants.TripSecurity.DescriptionMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.DescriptionMinLength)]
        public string Description { get; set; }
    }
}
