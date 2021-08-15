namespace Tripsters.Tests.Services
{
    using System.Threading.Tasks;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Xunit;

    using Tripsters.Data;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Users;
    using Tripsters.Data.Models;
    using System;
    using Tripsters.Services.Data.Users.Models;

    public class UsersServiceTests
    {
        [Fact]
        public async Task GetAllBadgesShouldThrowNullReferenceExceptionIfSomeUserNotExist()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId",
            };

            var anotherUser = new ApplicationUser
            {
                Id = "anoherUserId",
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var badgeService = new BadgesService(dbContext);
            var service = new UsersService(badgeService, dbContext);

            // Act
            // Assert

            await Assert.ThrowsAnyAsync<NullReferenceException>(async () =>
           {
               await service.AddFriend(user.Id, anotherUser.Id);
           });
        }

        [Fact]
        public async Task GetAllBadgesShouldReturnAllBadgesFromDataBase()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId",
            };

            var anotherUser = new ApplicationUser
            {
                Id = "anoherUserId",
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(anotherUser);
            dbContext.SaveChanges();

            var badgeService = new BadgesService(dbContext);
            var service = new UsersService(badgeService, dbContext);

            // Act
            await service.AddFriend(user.Id, anotherUser.Id);
            // Assert
            Assert.Equal(2, dbContext.UserFriends.Count());
        }

        [Fact]
        public async Task EditShouldThrowNullReferenceExceptionIfUserNotExist()
        {
            // Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var badgeService = new BadgesService(dbContext);
            var service = new UsersService(badgeService, dbContext);

            // Act
            // Assert

            await Assert.ThrowsAnyAsync<NullReferenceException>(async () =>
            {
                await service.Edit(new UserProfileServiceModel { UserId = "userId" });
            });
        }

        [Fact]
        public async Task EditShouldEditUserCorrectly()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId",
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var badgeService = new BadgesService(dbContext);
            var service = new UsersService(badgeService, dbContext);

            // Act
            var editUser = new UserProfileServiceModel
            {
                UserId = "userId",
                UserName = "testUser",
                Town = "testTown",
                Age = 1,
                PhoneNumber = "08881112222",
                Email = "test@test.test",
            };

            await service.Edit(editUser);

            var editedUser = dbContext.Users.FirstOrDefault();
            // Assert

            Assert.Equal(editUser.UserName, editedUser.UserName);
            Assert.Equal(editUser.Town, editedUser.Town);
            Assert.Equal(editUser.Age, editedUser.Age);
            Assert.Equal(editUser.PhoneNumber, editedUser.PhoneNumber);
            Assert.Equal(editUser.Email, editedUser.Email);
        }
    }
}
