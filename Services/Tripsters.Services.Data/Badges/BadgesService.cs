namespace Tripsters.Services.Data.Badges
{
    using System.Collections.Generic;
    using System.Linq;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;

    public class BadgesService : IBadgesService
    {
        private readonly IDeletableEntityRepository<Badge> badgeRepository;

        public BadgesService(IDeletableEntityRepository<Badge> badgeRepository)
        {
            this.badgeRepository = badgeRepository;
        }

        public ICollection<BadgeServiceModel> GetAllBadges()
        => this.badgeRepository.All()
            .OrderBy(b => b.Name)
            .Select(b => new BadgeServiceModel
            {
                Id = b.Id,
                Name = b.Name,
            })
            .ToList();

        public Badge GetBadgeById(int badgeId)
        => this.badgeRepository.All()
            .Where(b => b.Id == badgeId)
            .Select(b => new Badge
            {
                Id = b.Id,
                Name = b.Name,
            })
            .FirstOrDefault();
    }
}
