namespace Tripsters.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Tripsters.Web.ViewModels.Badges;

    public class UserProfileViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public string HomeTown { get; set; }

        public ICollection<UserViewModel> Friends { get; set; }

        public ICollection<BadgeViewModel> Badges { get; set; }
    }
}
