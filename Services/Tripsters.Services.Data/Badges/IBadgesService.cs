namespace Tripsters.Services.Data.Badges
{
    using System.Collections.Generic;

    using Tripsters.Data.Models;

    public interface IBadgesService
    {
        ICollection<BadgeServiceModel> GetAllBadges();

        Badge GetBadgeById(int badgeId);
    }
}
