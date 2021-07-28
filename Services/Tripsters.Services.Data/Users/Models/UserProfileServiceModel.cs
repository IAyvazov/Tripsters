namespace Tripsters.Services.Data.Users.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Photos;

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

        public ICollection<UserServiceModel> Friends { get; set; }

        [Display(Name = "Add Badge")]

        public ICollection<BadgeServiceModel> UserBadges { get; set; }

        public ICollection<BadgeServiceModel> AllBadges { get; set; }

        public ICollection<PhotoServiceModel> Photos { get; set; }
    }
}
