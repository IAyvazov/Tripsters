namespace Tripsters.Services.Data.Badges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

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

        public async Task AddBadgeToUser(int badgeId, string userId, string userWhoAddId)
        {
            var user = this.dbContext.Users
                .Where(u => u.Id == userId)
                .Include(x => x.Badges)
                .FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentNullException("There is no such user.");
            }

            var badge = this.GetBadgeById(badgeId);

            if (badge == null)
            {
                throw new ArgumentNullException("There is no such badge");
            }

            if (user.Badges.Any(b => b.BadgeId == badgeId))
            {
                return;
            }

            var userBadges = new UsersBadges { BadgeId = badgeId, UserId = userId, AdderId = userWhoAddId };

            await this.dbContext.UsersBadges.AddAsync(userBadges);

            await this.dbContext.SaveChangesAsync();
        }
    }
}
