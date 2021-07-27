namespace Tripsters.Services.Data.Badges
{
    using System.Collections.Generic;
    using System.Linq;

    using Tripsters.Data;
    using Tripsters.Data.Models;

    public class BadgesService : IBadgesService
    {
        private readonly ApplicationDbContext dbContext;

        public BadgesService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICollection<BadgeServiceModel> GetAllBadges()
        => this.dbContext.Badges
            .OrderBy(b => b.Name)
            .Select(b => new BadgeServiceModel
            {
                Id = b.Id,
                Name = b.Name,
            })
            .ToList();

        public Badge GetBadgeById(int badgeId)
        => this.dbContext.Badges
            .Where(b => b.Id == badgeId)
            .Select(b => new Badge
            {
                Id = b.Id,
                Name = b.Name,
            })
            .FirstOrDefault();
    }
}
