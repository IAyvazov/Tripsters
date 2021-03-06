namespace Tripsters.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Tripsters.Web.ViewModels.Badges;

    public class UserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public string CurrentTripId { get; set; }

        public ICollection<UserViewModel> MutualFriends { get; set; }

        public ICollection<BadgeViewModel> Badges { get; set; }
    }
}
