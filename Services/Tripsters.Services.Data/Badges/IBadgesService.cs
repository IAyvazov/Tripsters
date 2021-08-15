namespace Tripsters.Services.Data.Badges
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Data.Models;

    public interface IBadgesService
    {
        ICollection<BadgeServiceModel> GetAllBadges();

        Badge GetBadgeById(int badgeId);

        Task AddBadgeToUser(int badgeId, string userId, string userWhoAddId);
    }
}
