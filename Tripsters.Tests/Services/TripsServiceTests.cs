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

        [Fact]
        public async Task ApprovetShouldThrowExceptionIfTripIdIsIncorect()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);

            var service = new TripsService(dbContext);

            // Act
            // Assert

            await Assert.ThrowsAnyAsync<NullReferenceException>(async () =>
            {
                await service.Approve("incorectId");
            });
        }

        [Fact]
        public async Task ApproveShouldSetProperyIsApprovedToTrue()
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

            var trip = dbContext.Trips.FirstOrDefault();

            var result = await service.Approve(trip.Id);

            // Assert
            Assert.Equal(user.Id, result);
            Assert.True(trip.IsApproved);
        }


        [Fact]
        public async Task GetAllTripsCountShouldReturnAllTripsCount()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");

            var allTripsCount = service.GetAllTripsCount();

            await service.Approve(dbContext.Trips.FirstOrDefault().Id);

            var allApprovedTripsCount = service.GetAllTripsCount();

            // Assert
            Assert.Equal(0, allTripsCount);
            Assert.Equal(1, allApprovedTripsCount);
        }


        [Fact]
        public async Task GetAllUsersTripsCountShouldReturnAllTripsCount()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");
            await service.AddTrip(tripData, "userId");

            var allTripsCount = service.GetAllUserTripsCount("userId");

            await service.Approve(dbContext.Trips.FirstOrDefault().Id);
            await service.Approve(dbContext.Trips.LastOrDefault().Id);

            var allApprovedTripsCount = service.GetAllUserTripsCount("userId");

            // Assert
            Assert.Equal(0, allTripsCount);
            Assert.Equal(2, allApprovedTripsCount);
        }

        [Fact]
        public async Task JoinTripShouldThrowInvalidOperationExceptionIfUserIsAlreadyJoined()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId",
            };

            var newUser = new ApplicationUser
            {
                Id = "newUserId",
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");

            var tripId = dbContext.Trips.FirstOrDefault().Id;

            await service.JoinTrip(tripId, newUser.Id);
            // Assert
            Assert.Single(dbContext.UserTrips);
            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
            {
                await service.JoinTrip(tripId, newUser.Id);
            });
        }

        [Fact]
        public async Task JoinTripShouldAddUserInTripMembers()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId",
            };

            var newUser = new ApplicationUser
            {
                Id = "newUserId",
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");

            var tripId = dbContext.Trips.FirstOrDefault().Id;

            await service.JoinTrip(tripId, newUser.Id);
            // Assert
            Assert.Single(dbContext.UserTrips);
        }

        [Fact]
        public async Task JoinTripShouldThrowInvalidOperationExceptionIfAvailableSeatsAreZero()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = "userId",
            };

            var newUser = new ApplicationUser
            {
                Id = "newUserId",
            };

            var anotherUser = new ApplicationUser
            {
                Id = "anotherUser",
            };
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("test");

            var dbContext = new ApplicationDbContext(optionsBuilder.Options);
            dbContext.Users.Add(user);
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 1,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");

            var tripId = dbContext.Trips.FirstOrDefault().Id;

            await service.JoinTrip(tripId, newUser.Id);
            // Assert

            Assert.Equal(0, dbContext.Trips.FirstOrDefault().AvailableSeats);

            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
            {
                await service.JoinTrip(tripId, anotherUser.Id);
            });
        }

        [Fact]
        public async Task JoinTripShouldThrowInvalidOperationExceptionIfUserWhoMadeTheTripTryToJoinTheTrip()
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");

            var tripId = dbContext.Trips.FirstOrDefault().Id;

            // Assert

            await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>
            {
                await service.JoinTrip(tripId, user.Id);
            });
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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");
            var tripId = dbContext.Trips.FirstOrDefault().Id;

            var result = await service.LikeTrip(tripId, user.Id);

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

            var service = new TripsService(dbContext);
            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");
            var tripId = dbContext.Trips.FirstOrDefault().Id;

            var result = await service.LikeTrip(tripId, user.Id);
            await service.LikeTrip(tripId, user.Id);

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

            var service = new TripsService(dbContext);

            var tripData = new TripServiceFormModel
            {
                AvailableSeats = 2,
                Name = "TripName",
                Description = "TripDescription",
                StartDate = DateTime.Now.AddDays(1),
                CategoryId = 1,
            };

            // Act
            await service.AddTrip(tripData, "userId");
            var tripId = dbContext.Trips.FirstOrDefault().Id;

            await service.LikeTrip(tripId, anotherUser.Id);
            var result = await service.LikeTrip(tripId, user.Id);

            // Assert
            Assert.Equal(2, result);
        }
    }
}
