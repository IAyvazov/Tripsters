namespace Tripsters.Tests.Services
{
    using System.Threading.Tasks;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    using Tripsters.Data;
    using Tripsters.Services.Data.Photos;
    using Tripsters.Data.Models;
    using System;

    public class PhotosServiceTests
    {
        [Fact]
        public async Task AddPhotoShouldAddPhotoPathToDataBase()
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

            var service = new PhotosService(dbContext);

            // Act
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00615.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00617.JPG", "userId");

            // Assert

            Assert.NotNull(dbContext.Photos);
            Assert.Equal(3, dbContext.Photos.Count());
        }

        [Fact]
        public async Task AddPhotoShouldThrowException()
        {
            // Arrange

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var service = new PhotosService(dbContext);

            // Act
            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(async () =>
             {
                 await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
             });
        }

        [Fact]
        public async Task DeletePhotoShouldSetPhotoIsDeletedPropertyToTrue()
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

            var service = new PhotosService(dbContext);

            // Act
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00615.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00617.JPG", "userId");

            var result = await service.DeletePhoto(1, "userId");

            // Assert
            Assert.True(result);
            Assert.NotNull(dbContext.Photos);
            Assert.Equal(2, dbContext.Photos.Count());
        }

        [Fact]
        public async Task DeletePhotoShouldReturnFalseIfThereIsNoSuchUserId()
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

            var service = new PhotosService(dbContext);

            // Act
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00615.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00617.JPG", "userId");

            var result = await service.DeletePhoto(1, "user");

            // Assert
            Assert.False(result);
            Assert.NotNull(dbContext.Photos);
            Assert.Equal(3, dbContext.Photos.Count());
        }

        [Fact]
        public async Task LikeShouldIncreesWithOneAndReturnCountOfLikes()
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

            var service = new PhotosService(dbContext);

            // Act
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00615.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00617.JPG", "userId");

            await service.Like(1, "userId");
            var result = await service.Like(1, "userId");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task LikeShouldIncreesWithOneIfOnlyOneUserLikeThePhotoAndReturnCountOfLikes()
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

            var service = new PhotosService(dbContext);

            // Act
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00615.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00617.JPG", "userId");

            await service.Like(1, "userId");
            var result = await service.Like(1, "userId");

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task LikeShouldIncreesWithTwoIfTwoUsersLikeThePhotoAndReturnCountOfLikes()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId"
            };

            var anotherUser = new ApplicationUser
            {
                Id = "anotherUserId"
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var service = new PhotosService(dbContext);

            // Act
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00616.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00615.JPG", "userId");
            await service.AddPhoto("C:\\Users\\iayva\\source\\repos\\Tripsters\\Web\\Tripsters.Web\\wwwroot\\Uploads\\DSC00617.JPG", "userId");

            await service.Like(1, "anotherUserId");
            var result = await service.Like(1, "userId");

            // Assert
            Assert.Equal(2, result);
        }
    }
}
