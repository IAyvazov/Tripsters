namespace Tripsters.Data.Models
{
    using Tripsters.Data.Common.Models;

    public class UsersBadges : BaseDeletableModel<int>
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int BadgeId { get; set; }

        public Badge Badge { get; set; }
    }
}
