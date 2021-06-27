namespace Tripsters.Web.ViewModels.Trips
{
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Common;

    public class TripsInputFormModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        [StringLength(GlobalConstants.Trip.NameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.Trip.NameMinLength)]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "From Town")]
        [StringLength(GlobalConstants.Trip.NameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.Trip.NameMinLength)]
        public string FromTown { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "To Town")]
        [StringLength(GlobalConstants.Trip.TownMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.Trip.TownMinLength)]
        public string ToTown { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Available Seats")]
        [Range(GlobalConstants.Trip.AvailableSeatsMinRange, GlobalConstants.Trip.AvailableSeatsMaxRange)]
        public int AvailableSeats { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Description")]
        [StringLength(GlobalConstants.Trip.DescriptionMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = GlobalConstants.Trip.DescriptionMinLength)]
        public string Description { get; set; }
    }
}
