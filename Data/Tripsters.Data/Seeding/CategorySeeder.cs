namespace Tripsters.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Data.Models;

    public class CategorySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
            {
                return;
            }

            await dbContext.Categories.AddAsync(new Category { Name = "Sky" });
            await dbContext.Categories.AddAsync(new Category { Name = "Mountain" });
            await dbContext.Categories.AddAsync(new Category { Name = "Water" });
        }
    }
}
