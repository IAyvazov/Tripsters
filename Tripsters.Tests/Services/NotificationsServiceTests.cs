namespace Tripsters.Tests.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    using Tripsters.Data;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Notifications;

    public class NotificationsServiceTests
    {
        [Fact]
        public void GetAllNotificationsShouldReturnAllNotificationsForUserFromDataBase()
        {
            // Arrange
            var notificationFalse = new Notification
            {
                UserId = "userId",
                IsSeen = false,
                FriendId = "friendId"
            };
            var notificationTrue = new Notification
            {
                UserId = "userId",
                IsSeen = true,
                FriendId = "currUserId"
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            dbContext.Notifications.Add(notificationFalse);
            dbContext.Notifications.Add(notificationTrue);
            dbContext.SaveChanges();

            var service = new NotificationsService(dbContext);

            // Act
            var result = service.GetAllNotification("userId");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);
        }

        [Fact]
        public async Task NotifieShouldSaveNotificationToUserInDataBase()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };
            var currUser = new ApplicationUser { Id = "currUserId" };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(currUser);
            dbContext.SaveChanges();

            var service = new NotificationsService(dbContext);

            // Act
            await service.Notifie("currUserId", "userId", "test notifiacation");

            // Assert
            Assert.NotNull(dbContext.Notifications);
            Assert.Equal(1, dbContext.Notifications.Count());
        }

        [Fact]
        public async Task SeenShouldSetNotificaionIsSeenPropertyToBeTrue()
        {
            // Arrange
            var user = new ApplicationUser { Id = "userId" };
            var currUser = new ApplicationUser { Id = "currUserId" };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(currUser);
            dbContext.SaveChanges();

            var service = new NotificationsService(dbContext);

            // Act
            await service.Notifie("currUserId", "userId", "test notifiacation");

            await service.Seen(1);

            // Assert
            Assert.Equal(1, dbContext.Notifications.Count());
            Assert.True(dbContext.Notifications.First().IsSeen);
        }
    }
}
