namespace Tripsters.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Xunit;

    using Tripsters.Data;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Trips;
    using Tripsters.Services.Data.Trips.Models;

    public class TripsServiceTests
    {
        [Fact]
        public async Task AddTripShouldAddTripInDataBase()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now,
                CategoryId = 1,
            };
            // Act
            await service.AddTrip(tripData, "userId");

            // Assert
            Assert.NotNull(dbContext.Trips);
            Assert.Equal(1, dbContext.Trips.Count());
        }

        [Fact]
        public async Task DeleteTripShouldSetTripPropertyIsDeletedToTrueInDataBase()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now,
                CategoryId = 1,
            };

            var newTripData = new TripServiceFormModel
            {
                AvailableSeats = 3,
                Name = "TripNameNEw",
                Description = "TripDescriptionNew",
                StartDate = DateTime.Now,
                CategoryId = 2,
            };
            // Act
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(newTripData, "userId");
            await service.AddTrip(newTripData, "userId");

            var firstTripId = dbContext.Trips.First().Id;
            var lastTripId = dbContext.Trips.Last().Id;

            await service.Delete(firstTripId);
            await service.Delete(lastTripId);

            // Assert
            Assert.NotNull(dbContext.Trips);
            Assert.Equal(2, dbContext.Trips.Count());
        }

        [Fact]
        public async Task EditTripShouldEditCorrectly()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now,
                CategoryId = 1,
            };

            var editTripData = new TripServiceFormModel
            {
                AvailableSeats = 3,
                Name = "TripNameEdit",
                Description = "TripDescriptionEdit",
                StartDate = DateTime.Now,
                CategoryId = 2,
            };
            // Act
            await service.AddTrip(tripData, "userId");

            var tripId = dbContext.Trips.First().Id;

            await service.EditTrip(tripId, editTripData);

            // Assert
            var editedTrip = dbContext.Trips.First();

            Assert.Equal(editTripData.Name, editedTrip.Name);
            Assert.Equal(editTripData.AvailableSeats, editedTrip.AvailableSeats);
            Assert.Equal(editTripData.Description, editedTrip.Description);
            Assert.Equal(editTripData.StartDate, editedTrip.StartDate);
            Assert.Equal(editTripData.CategoryId, editedTrip.CategoryId);
        }

        [Fact]
        public void AllCategoryTripShouldReturnAllCategoriesFromDataBase()
        {
            // Arrange
            var categorySky = new Category
            {
                Name = "sky"
            };

            var categoryWater = new Category
            {
                Name = "water"
            };

            var categoryMountain = new Category
            {
                Name = "mountain"
            };

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Categories.AddRange(categorySky, categoryWater, categoryMountain);
            dbContext.SaveChanges();

            var service = new TripsService(dbContext);

            // Act
            var categories = service.AllCategories();

            // Assert
            Assert.Equal(3, categories.Count());
            Assert.NotNull(categories);
        }

        [Fact]
        public async Task AddCommentShouldAddCommentToTheTrip()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now,
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");

            var tripId = dbContext.Trips.First().Id;

            await service.AddComment(user.Id, tripId, "some comment");

            // Assert
            Assert.NotNull(dbContext.Comments);
            Assert.Equal(1, dbContext.Comments.Count());
            Assert.Equal(tripId, dbContext.Comments.FirstOrDefault().TripId);
            Assert.Equal(user.Id, dbContext.Comments.FirstOrDefault().UserId);
        }

        [Fact]
        public async Task AddCommentShouldThrowExceptionIfTripIdIsIncorect()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now,
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");



            // Assert

            await Assert.ThrowsAnyAsync<NullReferenceException>(async () =>
            {
                await service.AddComment(user.Id, "", "some comment");
            });
        }
    }
}
