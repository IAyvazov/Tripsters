namespace Tripsters.Services.Data.Users.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Photos;
    using Tripsters.Services.Data.Trips.Models;

    public class UserProfileServiceModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Range(16, 120)]
        public int Age { get; set; }

        [Display(Name = "Phone Number")]
        [StringLength(15, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string Town { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int PhotosPerPage { get; } = 4;

        public int TotalPhotos { get; set; }

        [Display(Name = "Profile picture")]

        public IFormFile ProfilePicture { get; set; }

        public string ProfilePictureUrl { get; set; }

        public IEnumerable<TripServiceModel> RecentTrips { get; set; }

        public IEnumerable<UserServiceModel> Friends { get; set; }

        [Display(Name = "Add Badge")]

        public IEnumerable<BadgeServiceModel> UserBadges { get; set; }

        public IEnumerable<BadgeServiceModel> AllBadges { get; set; }

        public IEnumerable<PhotoServiceModel> Photos { get; set; }
    }
}
