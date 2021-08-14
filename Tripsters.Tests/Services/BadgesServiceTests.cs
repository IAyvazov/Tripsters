namespace Tripsters.Tests.Services
{
    using Microsoft.EntityFrameworkCore;
    using Tripsters.Data;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Xunit;

    public class BadgesServiceTests
    {
        [Fact]
        public void GetAllBadgesShouldReturnAllBadgesFromDataBase()
        {
            // Arrange
            var badge = new Badge
            {
                Name = "Test",
                Id = 1,
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            dbContext.Badges.Add(badge);
            dbContext.SaveChanges();

            var service = new BadgesService(dbContext);

            // Act
            var result = service.GetAllBadges();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void GetBadgeByIdShouldReturnCorrectBadge()
        {
            // Arrange
            var badge = new Badge
            {
                Name = "Test",
                Id = 2,
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            dbContext.Badges.Add(badge);
            dbContext.SaveChanges();

            var service = new BadgesService(dbContext);

            // Act
            var result = service.GetBadgeById(2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);
        }

        [Fact]
        public void GetBadgeByIdShouldReturnNullIFDoesNotExis()
        {
            // Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var service = new BadgesService(dbContext);

            // Act
            var result = service.GetBadgeById(1);

            // Assert
            Assert.Null(result);
        }
    }
}
