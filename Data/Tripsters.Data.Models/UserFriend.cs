namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class UserFriend : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string FriendId { get; set; }

        public ApplicationUser Friend { get; set; }
    }
}
