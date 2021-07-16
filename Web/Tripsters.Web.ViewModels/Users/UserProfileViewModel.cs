namespace Tripsters.Web.ViewModels.Users
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Tripsters.Web.ViewModels.Badges;

    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Range(16, 120)]
        public int Age { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 4)]
        [Display(Name = "Town")]
        [DataType(DataType.Text)]
        public string HomeTown { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Profile picture")]
        public string ProfilePictureUrl { get; set; }

        public ICollection<UserViewModel> Friends { get; set; }

        public ICollection<BadgeViewModel> Badges { get; set; }
    }
}
