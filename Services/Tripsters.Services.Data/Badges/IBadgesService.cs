namespace Tripsters.Services.Data.Badges
{
    using System.Collections.Generic;

    using Tripsters.Data.Models;
    using Tripsters.Web.ViewModels.Badges;

    public interface IBadgesService
    {
        ICollection<BadgeViewModel> GetAllBadges();

        Badge GetBadgeById(string badgeId);
    }
}
