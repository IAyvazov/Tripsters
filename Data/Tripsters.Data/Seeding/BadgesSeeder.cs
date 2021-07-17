namespace Tripsters.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Data.Models;

    public class BadgesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Badges.Any())
            {
                return;
            }

            await dbContext.Badges.AddAsync(new Badge { Name = "Adventurer" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Crazy" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Clumsy" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Happy" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Talktive" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Angry" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Lover" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Grumbler" });
            await dbContext.Badges.AddAsync(new Badge { Name = "Funny" });
        }
    }
}
