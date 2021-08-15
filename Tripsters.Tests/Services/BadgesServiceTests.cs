namespace Tripsters.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Xunit;

    using Tripsters.Data;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;

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

        [Fact]
        public async Task AddBadgeToUserShouldThrowExceptionIfThereIsNoSuchUser()
        {
            // Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var service = new BadgesService(dbContext);

            // Act

            // Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(async () =>
            {
                await service.AddBadgeToUser(1, "userId", "userWhoAddId");
            });
        }

        [Fact]
        public async Task AddBadgeToUserShouldThrowExceptionIfThereIsNoSuchBadge()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId"
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var service = new BadgesService(dbContext);

            // Act
            // Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(async () =>
            {
                await service.AddBadgeToUser(1, "userId", "userWhoAddId");
            });
        }

        [Fact]
        public async Task AddBadgeToUserShouldAddBadgeOnlyOnceFromOneUser()
        {
            // Arrange
            var badge = new Badge
            {
                Name = "Test",
                Id = 1,
            };

            var user = new ApplicationUser
            {
                Id = "userId"
            };

            var newUser = new ApplicationUser
            {
                Id = "newUser"
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(newUser);
            dbContext.Badges.Add(badge);
            dbContext.SaveChanges();

            var service = new BadgesService(dbContext);

            // Act
            await service.AddBadgeToUser(badge.Id, user.Id, newUser.Id);
            await service.AddBadgeToUser(badge.Id, user.Id, newUser.Id);

            // Assert
            Assert.NotNull(dbContext.UsersBadges);
            Assert.Single(dbContext.UsersBadges);
        }

        [Fact]
        public async Task AddBadgeToUserShouldAddBadgesFromDifferntUsers()
        {
            // Arrange
            var badge = new Badge
            {
                Name = "Test",
                Id = 1,
            };

            var testBadge = new Badge
            {
                Name = "TestBadge",
                Id = 2,
            };

            var user = new ApplicationUser
            {
                Id = "userId"
            };

            var newUser = new ApplicationUser
            {
                Id = "newUser"
            };

            var testUser = new ApplicationUser
            {
                Id = "testUser"
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(newUser);
            dbContext.Users.Add(testUser);
            dbContext.Badges.Add(badge);
            dbContext.Badges.Add(testBadge);
            dbContext.SaveChanges();

            var service = new BadgesService(dbContext);

            // Act
            await service.AddBadgeToUser(badge.Id, user.Id, newUser.Id);
            await service.AddBadgeToUser(testBadge.Id, user.Id, testUser.Id);

            // Assert
            Assert.NotNull(dbContext.UsersBadges);
            Assert.Equal(2, dbContext.UsersBadges.Count());
        }
    }
}
