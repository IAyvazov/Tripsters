namespace Tripsters.Services.Data.Users.Models
{
    using System.Collections.Generic;

    using Tripsters.Services.Data.Badges;

    public class UserServiceModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public int Age { get; set; }

        public string CurrentTripId { get; set; }

        public ICollection<UserServiceModel> MutualFriends { get; set; }

        public ICollection<BadgeServiceModel> Badges { get; set; }
    }
}
