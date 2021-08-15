namespace Tripsters.Services.Data.Trips
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Tripsters.Data;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Services.Data.Users.Models;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext dbContext;

        public TripsService(
            ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddTrip(TripServiceFormModel tripData, string userId)
        {
            var trip = new Trip
            {
                AvailableSeats = tripData.AvailableSeats,
                Name = tripData.Name,
                Description = tripData.Description,
                StartDate = tripData.StartDate,
                CategoryId = tripData.CategoryId,
                UserId = userId,
            };

            trip.Destination = new Destination { From = tripData.From, To = tripData.To, TripId = trip.Id };

            await this.dbContext.Trips.AddAsync(trip);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task Delete(string tripId)
        {
            var trip = this.dbContext.Trips
                .Where(t => t.Id == tripId)
                .FirstOrDefault().IsDeleted = true;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task EditTrip(string tripId, TripServiceFormModel tripData)
        {
            var trip = this.dbContext.Trips
                 .Where(t => t.Id == tripId)
                 .Include(d => d.Destination)
                 .FirstOrDefault();

            trip.Name = tripData.Name;
            trip.AvailableSeats = tripData.AvailableSeats;
            trip.Description = tripData.Description;
            trip.StartDate = tripData.StartDate;
            trip.Destination.From = tripData.From;
            trip.Destination.To = tripData.To;
            trip.CategoryId = tripData.CategoryId;

            await this.dbContext.SaveChangesAsync();
        }

        public IEnumerable<TripCategoryServiceModel> AllCategories()
           => this.dbContext
               .Categories
               .Select(c => new TripCategoryServiceModel
               {
                   Id = c.Id,
                   Name = c.Name,
               })
               .ToList();

        public async Task AddComment(string userId, string tripId, string text)
        {
            var comment = new Comment
            {
                TripId = tripId,
                Text = text,
                UserId = userId,
            };

            var trip = this.dbContext.Trips
                .Where(t => t.Id == tripId)
                .FirstOrDefault();

            if (trip == null)
            {
                throw new NullReferenceException("There is no trip");
            }

            trip.Comments.Add(comment);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<string> Approve(string tripId)
        {
            var trip = this.dbContext.Trips.Where(t => t.Id == tripId).FirstOrDefault();

            trip.IsApproved = true;

            await this.dbContext.SaveChangesAsync();

            return trip.UserId;
        }

        public ICollection<TripServiceModel> GetAllTripsForAdmin(int currentPage, int tripsPerPage)
       => this.dbContext.Trips
           .Where(t => t.IsDeleted == false && t.StartDate >= DateTime.UtcNow)
           .OrderBy(t => t.StartDate)
           .Skip((currentPage - 1) * tripsPerPage)
               .Take(tripsPerPage)
           .Select(t => new TripServiceModel
           {
               Id = t.Id,
               Name = t.Name,
               From = t.Destination.From,
               To = t.Destination.To,
               AvailableSeats = t.AvailableSeats,
               Description = t.Description,
               CreatorName = t.User.UserName,
               StartDate = t.StartDate.ToString("G"),
               CreatorId = t.UserId,
               CategoryName = t.Category.Name,
               IsApproved = t.IsApproved,
           })
           .ToList();

        public ICollection<TripServiceModel> GetAllTrips(int currentPage, int tripsPerPage)
        => this.dbContext.Trips
            .Where(t => t.IsDeleted == false && t.StartDate >= DateTime.UtcNow && t.IsApproved)
            .OrderBy(t => t.StartDate)
            .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
            .Select(t => new TripServiceModel
            {
                Id = t.Id,
                Name = t.Name,
                From = t.Destination.From,
                To = t.Destination.To,
                AvailableSeats = t.AvailableSeats,
                Description = t.Description,
                CreatorName = t.User.UserName,
                StartDate = t.StartDate.ToString("G"),
                CreatorId = t.UserId,
                CategoryName = t.Category.Name,
                Members = t.Travellers
                .Select(m => new UserServiceModel
                {
                    Id = m.User.Id,
                    UserName = m.User.UserName,
                    Age = m.User.Age,
                    CurrentTripId = t.Id,
                    Badges = m.User.Badges
                    .Select(b => new BadgeServiceModel
                    {
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                        AdderId = b.AdderId,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> GetAllUserTrips(string userId, int currentPage, int tripsPerPage)
        => this.dbContext.Trips
            .Where(t => t.UserId == userId && t.IsDeleted == false && t.IsApproved)
            .OrderBy(t => t.StartDate)
            .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
            .Select(t => new TripServiceModel
            {
                Id = t.Id,
                CreatorId = userId,
                AvailableSeats = t.AvailableSeats,
                CreatorName = t.User.UserName,
                Description = t.Description,
                From = t.Destination.From,
                To = t.Destination.To,
                CategoryName = t.Category.Name,
                StartDate = t.StartDate.ToString("G"),
                Name = t.Name,
                Members = t.Travellers
                .Select(u => new UserServiceModel
                {
                    Id = u.User.Id,
                    UserName = u.User.UserName,
                    Age = u.User.Age,
                    Badges = u.User.Badges
                    .Select(b => new BadgeServiceModel
                    {
                        Name = b.Badge.Name,
                        Id = b.Badge.Id,
                        AdderId = b.AdderId,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> GetAllTripsByCategoryId(int categoryId, int currentPage, int tripsPerPage)
        => this.dbContext.Trips
            .Where(t => t.CategoryId == categoryId && t.IsDeleted == false && t.IsApproved)
            .OrderBy(t => t.StartDate)
            .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
            .Select(t => new TripServiceModel
            {
                Id = t.Id,
                AvailableSeats = t.AvailableSeats,
                CreatorName = t.User.UserName,
                Description = t.Description,
                From = t.Destination.From,
                To = t.Destination.To,
                CategoryName = t.Category.Name,
                StartDate = t.StartDate.ToString("G"),
                Name = t.Name,
                Members = t.Travellers
                .Select(u => new UserServiceModel
                {
                    Id = u.User.Id,
                    UserName = u.User.UserName,
                    Age = u.User.Age,
                    Badges = u.User.Badges
                    .Select(b => new BadgeServiceModel
                    {
                        Name = b.Badge.Name,
                        Id = b.Badge.Id,
                        AdderId = b.AdderId,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> RecentTrips(string userId, int currentPage, int tripsPerPage)
       => this.dbContext.UserTrips
           .Where(u => u.Trip.IsDeleted == false && (u.Trip.UserId == userId || u.Trip.Travellers.Any(t => t.UserId == userId)) && u.Trip.StartDate.Date.DayOfYear.CompareTo(DateTime.Today.DayOfYear) < 0 && u.Trip.IsApproved)
           .Skip((currentPage - 1) * tripsPerPage)
               .Take(tripsPerPage)
           .Select(t => new TripServiceModel
           {
               Id = t.TripId,
               Name = t.Trip.Name,
               From = t.Trip.Destination.From,
               To = t.Trip.Destination.To,
               CategoryName = t.Trip.Category.Name,
               CreatorName = t.Trip.User.UserName,
               CreatorId = t.Trip.UserId,
               Description = t.Trip.Description,
               Likes = t.Trip.Likes.Count,
               Comments = t.Trip.Comments,
               Members = t.Trip.Travellers
               .Select(m => new UserServiceModel
               {
                   Id = m.UserId,
                   UserName = m.User.UserName,
                   Age = m.User.Age,
                   Badges = m.User.Badges
                   .Select(b => new BadgeServiceModel
                   {
                       Id = b.Badge.Id,
                       Name = b.Badge.Name,
                       AdderId = b.AdderId,
                   }).ToList(),
               }).ToList(),
           }).ToList();

        public ICollection<TripServiceModel> GetPastTrips(string userId, int currentPage, int tripsPerPage)
        => this.dbContext.UserTrips
            .Where(u => u.Trip.IsDeleted == false && u.Trip.Travellers.Any(tr => tr.UserId == userId) && u.Trip.StartDate.Date.DayOfYear.CompareTo(DateTime.Today.DayOfYear) < 0 && u.Trip.IsApproved)
            .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
            .Select(t => new TripServiceModel
            {
                Id = t.TripId,
                Name = t.Trip.Name,
                From = t.Trip.Destination.From,
                To = t.Trip.Destination.To,
                CategoryName = t.Trip.Category.Name,
                CreatorName = t.Trip.User.UserName,
                CreatorId = t.Trip.UserId,
                Description = t.Trip.Description,
                Likes = t.Trip.Likes.Count,
                Comments = t.Trip.Comments,
                Members = t.Trip.Travellers
                .Select(m => new UserServiceModel
                {
                    Id = m.UserId,
                    UserName = m.User.UserName,
                    Age = m.User.Age,
                    Badges = m.User.Badges
                    .Select(b => new BadgeServiceModel
                    {
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                        AdderId = b.AdderId,
                    }).ToList(),
                }).ToList(),
            }).ToList();

        public ICollection<CommentViewModel> GetAllTripComments(string tripId)
       => this.dbContext.Trips
            .Where(t => t.IsDeleted == false && t.Id == tripId && t.IsApproved)
           .SelectMany(c => c.Comments)
            .Select(c => new CommentViewModel
            {
                UserId = c.UserId,
                Text = c.Text,
                UserName = c.User.UserName,
                TripId = c.TripId,
                UserImg = c.User.Photos
                .Where(p => p.IsProfilePicture)
                .Select(x => x.Url)
                .FirstOrDefault(),
                CreatedOn = c.CreatedOn.ToString("G"),
            })
            .ToList();

        public TripServiceModel GetTripById(string tripId, string userId)
        => this.dbContext.Trips
            .Where(t => t.Id == tripId && t.IsDeleted == false && t.IsApproved)
            .Select(t => new TripServiceModel
            {
                Id = t.Id,
                Name = t.Name,
                AvailableSeats = t.AvailableSeats,
                From = t.Destination.From,
                To = t.Destination.To,
                Description = t.Description,
                StartDate = t.StartDate.ToString("G"),
                CreatorName = t.User.UserName,
                CreatorId = t.UserId,
                CurrentUserId = userId,
                Comments = t.Comments,
                CategoryName = t.Category.Name,
                Members = t.Travellers
                .Select(m => new UserServiceModel
                {
                    Id = m.User.Id,
                    UserName = m.User.UserName,
                    Age = m.User.Age,
                    CurrentTripId = t.Id,
                    Badges = m.User.Badges
                    .Select(b => new BadgeServiceModel
                    {
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                        AdderId = b.AdderId,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .FirstOrDefault();

        public ICollection<TripServiceModel> GetUpcommingTodayTrips(string userId)
        => this.dbContext.Trips
            .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear.CompareTo(DateTime.Today.DayOfYear) == 0 && t.IsApproved)
            .Select(t => new TripServiceModel
            {
                Id = t.Id,
                Name = t.Name,
                From = t.Destination.From,
                To = t.Destination.To,
                CreatorName = t.User.UserName,
                Description = t.Description,
                CategoryName = t.Category.Name,
                StartDate = t.StartDate.ToString("G"),
                Members = t.Travellers
               .Select(m => new UserServiceModel
               {
                   Id = m.UserId,
                   UserName = m.User.UserName,
                   Age = m.User.Age,
                   CurrentTripId = t.Id,
                   Badges = m.User.Badges
                   .Select(b => new BadgeServiceModel
                   {
                       Id = b.BadgeId,
                       Name = b.Badge.Name,
                       AdderId = b.AdderId,
                   }).ToList(),
               }).ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> GetUpcommingTomorrowTrips(string userId)
       => this.dbContext.Trips
           .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear - DateTime.UtcNow.DayOfYear == 1 && t.IsApproved)
           .Select(t => new TripServiceModel
           {
               Id = t.Id,
               Name = t.Name,
               From = t.Destination.From,
               To = t.Destination.To,
               CreatorName = t.User.UserName,
               Description = t.Description,
               CategoryName = t.Category.Name,
               StartDate = t.StartDate.ToString("G"),
               Members = t.Travellers
               .Select(m => new UserServiceModel
               {
                   Id = m.UserId,
                   UserName = m.User.UserName,
                   Age = m.User.Age,
                   CurrentTripId = t.Id,
                   Badges = m.User.Badges
                   .Select(b => new BadgeServiceModel
                   {
                       Id = b.BadgeId,
                       Name = b.Badge.Name,
                       AdderId = b.AdderId,
                   }).ToList(),
               }).ToList(),
           })
           .ToList();

        public async Task JoinTrip(string tripId, string userId)
        {
            if (!this.dbContext.UserTrips.Any(u => u.UserId == userId && u.TripId == tripId && u.IsDeleted == false))
            {
                var userTrip = new UserTrip
                {
                    TripId = tripId,
                    UserId = userId,
                };

                if (this.dbContext.Trips.FirstOrDefault(t => t.Id == tripId && t.IsDeleted == false).AvailableSeats > 0)
                {
                    this.dbContext.Trips.FirstOrDefault(t => t.Id == tripId && t.IsDeleted == false).AvailableSeats--;
                }
                else
                {
                    throw new InvalidOperationException("There is no more avalable seats.");
                }

                await this.dbContext.UserTrips.AddAsync(userTrip);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<int> LikeTrip(string tripId, string userId)
        {
            var tripLike = this.dbContext.Trips
                .Where(t => t.Likes.Any(l => l.TripId == tripId && l.UserId == userId))
                .FirstOrDefault();

            if (tripLike == null)
            {
                var like = new Like
                {
                    TripId = tripId,
                    UserId = userId,
                };

                var trip = this.dbContext.Trips
                    .Where(t => t.Id == tripId)
                    .FirstOrDefault();

                trip.Likes.Add(like);

                await this.dbContext.SaveChangesAsync();

                return trip.Likes.Count;
            }

            return tripLike.Likes.Count;
        }

        public int GetAllTripsCount()
        => this.dbContext.Trips
            .Where(t => t.IsDeleted == false && t.StartDate >= DateTime.UtcNow && t.IsApproved)
            .Count();

        public int GetAllUserTripsCount(string userId)
        => this.dbContext.Trips
            .Where(t => t.UserId == userId && t.IsDeleted == false && t.IsApproved)
            .Count();
    }
}
