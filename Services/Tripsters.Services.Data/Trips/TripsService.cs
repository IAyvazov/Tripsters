namespace Tripsters.Services.Data.Trips
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly IDeletableEntityRepository<Trip> tripRepository;
        private readonly IDeletableEntityRepository<UserTrip> userTripRepository;
        private readonly IUsersService usersService;

        public TripsService(
            IDeletableEntityRepository<Trip> tripRepository,
            IDeletableEntityRepository<UserTrip> userTripRepository,
            IUsersService usersService)
        {
            this.tripRepository = tripRepository;
            this.userTripRepository = userTripRepository;
            this.usersService = usersService;
        }

        public async Task AddComment(string userId, string tripId, string commentInput)
        {
            var comment = new Comment
            {
                TripId = tripId,
                Text = commentInput,
                UserId = userId,
            };

            var comments = this.tripRepository.All()
                .Where(t => t.Id == tripId)
                .FirstOrDefault();

            comments.Comments.Add(comment);
            await this.tripRepository.SaveChangesAsync();
        }

        public async Task AddTrip(TripsInputFormModel tripData, string userId)
        {
            var trip = new Trip
            {
                AvailableSeats = tripData.AvailableSeats,
                Name = tripData.Name,
                Description = tripData.Description,
                StartDate = tripData.StartDate,
                Destination = new Destination { From = tripData.From, To = tripData.To },
            };

            var user = this.usersService.GetUser(userId);
            trip.User = user ?? throw new ArgumentNullException("You are not logged.");

            await this.tripRepository.AddAsync(trip);
            await this.tripRepository.SaveChangesAsync();
        }

        public async Task Delete(string tripId)
        {
            var trip = this.tripRepository.All()
                .Where(t => t.Id == tripId)
                .FirstOrDefault().IsDeleted = true;

            await this.tripRepository.SaveChangesAsync();
        }

        public async Task EditTrip(string tripId, TripsInputFormModel tripData)
        {
            var trip = this.tripRepository.All()
                 .Where(t => t.Id == tripId)
                 .FirstOrDefault();

            trip.Name = tripData.Name;
            trip.AvailableSeats = tripData.AvailableSeats;
            trip.Description = tripData.Description;
            trip.StartDate = tripData.StartDate;
            trip.Destination.From = tripData.From;
            trip.Destination.To = tripData.To;

            await this.tripRepository.SaveChangesAsync();
        }

        public ICollection<TripServiceModel> GetAllTrips(int currentPage, int tripsPerPage)
        => this.tripRepository.All()
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
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> GetAllUserTrips(string userId, int currentPage, int tripsPerPage)
        => this.tripRepository.All()
            .Where(t => t.UserId == userId && t.IsDeleted == false)
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
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> GetPastTrips(string userId, int currentPage, int tripsPerPage)
        => this.userTripRepository.All()
            .Where(u => u.Trip.Travellers.Any(tr => tr.UserId == userId) && u.Trip.StartDate.Date.DayOfYear.CompareTo(DateTime.Today.DayOfYear) < 0)
            .Skip((currentPage - 1) * tripsPerPage)
                .Take(tripsPerPage)
            .Select(t => new TripServiceModel
            {
                Id = t.TripId,
                Name = t.Trip.Name,
                From = t.Trip.Destination.From,
                To = t.Trip.Destination.To,
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
                    }).ToList(),
                }).ToList(),
            }).ToList();

        public ICollection<CommentViewModel> GetAllTripComments(string tripId)
       => this.tripRepository.All()
            .Where(t => t.IsDeleted == false && t.Id == tripId)
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
        => this.tripRepository.All()
            .Where(t => t.Id == tripId && t.IsDeleted == false)
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
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .FirstOrDefault();

        public ICollection<TripServiceModel> GetUpcommingTodayTrips(string userId)
        => this.tripRepository.All()
            .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear.CompareTo(DateTime.Today.DayOfYear) == 0)
            .Select(t => new TripServiceModel
            {
                Id = t.Id,
                Name = t.Name,
                From = t.Destination.From,
                To = t.Destination.To,
                CreatorName = t.User.UserName,
                Description = t.Description,
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
                   }).ToList(),
               }).ToList(),
            })
            .ToList();

        public ICollection<TripServiceModel> GetUpcommingTomorrowTrips(string userId)
       => this.tripRepository.All()
           .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear - DateTime.UtcNow.DayOfYear == 1)
           .Select(t => new TripServiceModel
           {
               Id = t.Id,
               Name = t.Name,
               From = t.Destination.From,
               To = t.Destination.To,
               CreatorName = t.User.UserName,
               Description = t.Description,
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
                   }).ToList(),
               }).ToList(),
           })
           .ToList();

        public async Task JoinTrip(string tripId, string userId)
        {
            if (!this.userTripRepository.All().Any(u => u.UserId == userId && u.TripId == tripId && u.IsDeleted == false))
            {
                var userTrip = new UserTrip
                {
                    TripId = tripId,
                    UserId = userId,
                };

                if (this.tripRepository.All().FirstOrDefault(t => t.Id == tripId && t.IsDeleted == false).AvailableSeats > 0)
                {
                    this.tripRepository.All().FirstOrDefault(t => t.Id == tripId && t.IsDeleted == false).AvailableSeats--;
                }
                else
                {
                    throw new InvalidOperationException("There is no more avalable seats.");
                }

                await this.userTripRepository.AddAsync(userTrip);
                await this.userTripRepository.SaveChangesAsync();
            }
        }

        public async Task<int> LikeTrip(string tripId, string userId)
        {
            var tripLike = this.tripRepository.All()
                .Where(t => t.Likes.Any(l => l.TripId == tripId && l.UserId == userId))
                .FirstOrDefault();

            if (tripLike == null)
            {
                var like = new Like
                {
                    TripId = tripId,
                    UserId = userId,
                };

                var trip = this.tripRepository.All()
                    .Where(t => t.Id == tripId)
                    .FirstOrDefault();

                trip.Likes.Add(like);

                await this.tripRepository.SaveChangesAsync();

                return trip.Likes.Count();
            }

            return tripLike.Likes.Count();
        }

        public int GetAllTripsCount()
        => this.tripRepository.All()
            .Where(t => t.IsDeleted == false && t.StartDate >= DateTime.UtcNow)
            .Count();

        public int GetAllUserTripsCount(string userId)
        => this.tripRepository.All()
            .Where(t => t.UserId == userId && t.IsDeleted == false)
            .Count();
    }
}
