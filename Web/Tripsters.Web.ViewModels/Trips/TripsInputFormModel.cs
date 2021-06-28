namespace Tripsters.Web.ViewModels.Trips
{
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Common;

    public class TripsInputFormModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(GlobalConstants.TripSecurity.NameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.NameMinLength)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "From Town")]
        [StringLength(GlobalConstants.TripSecurity.TownMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.TownMinLength)]
        public string FromTown { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "To Town")]
        [StringLength(GlobalConstants.TripSecurity.TownMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.TripSecurity.TownMinLength)]
        public string ToTown { get; set; }

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
