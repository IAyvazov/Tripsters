namespace Tripsters.Services.Data.Trips
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Towns;
    using Tripsters.Services.Data.Users;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Trips;
    using Tripsters.Web.ViewModels.Users;

    using static Tripsters.Common.GlobalConstants;

    public class TripsService : ITripsService
    {
        private readonly IDeletableEntityRepository<Trip> tripRepository;
        private readonly IDeletableEntityRepository<UserTrip> userTripRepository;
        private readonly ITownsService townsService;
        private readonly IUsersService usersService;

        public TripsService(
            IDeletableEntityRepository<Trip> tripRepository,
            IDeletableEntityRepository<UserTrip> userTripRepository,
            ITownsService townsService,
            IUsersService usersService)
        {
            this.tripRepository = tripRepository;
            this.userTripRepository = userTripRepository;
            this.townsService = townsService;
            this.usersService = usersService;
        }

        public async Task AddTrip(TripsInputFormModel tripData, string userName)
        {
            var trip = new Trip
            {
                AvailableSeats = tripData.AvailableSeats,
                Name = tripData.Name,
                Description = tripData.Description,
                StartDate = tripData.StartDate,
            };

            var fromTown = this.townsService.GetTownByName(tripData.FromTown);

            if (fromTown == null)
            {
                fromTown = new Town
                {
                    Name = tripData.FromTown,
                };
            }

            var toTown = this.townsService.GetTownByName(tripData.ToTown);

            if (toTown == null)
            {
                toTown = new Town
                {
                    Name = tripData.ToTown,
                };
            }

            var user = this.usersService.GetUser(userName);
            trip.FromTown = fromTown;
            trip.ToTown = toTown;
            trip.User = user ?? throw new ArgumentNullException("You are not loggin.");

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

        public async Task EditTrip(TripsViewModel tripData)
        {
            var trip = this.tripRepository.All()
                 .Where(t => t.Id == tripData.Id)
                 .FirstOrDefault();

            DateTime startDate;
            DateTime.TryParseExact(tripData.StartDate, "G", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate);

            var fromTown = this.townsService.GetTownByName(tripData.FromTown);

            if (fromTown == null)
            {
                fromTown = new Town
                {
                    Name = tripData.FromTown,
                };
            }

            var toTown = this.townsService.GetTownByName(tripData.ToTown);

            if (toTown == null)
            {
                toTown = new Town
                {
                    Name = tripData.ToTown,
                };
            }

            trip.Name = tripData.Name;
            trip.FromTown = fromTown;
            trip.ToTown = toTown;
            trip.AvailableSeats = tripData.AvailableSeats;
            trip.Description = tripData.Description;
            trip.StartDate = startDate;

            await this.tripRepository.SaveChangesAsync();
        }

        public ICollection<TripsViewModel> GetAllTrips()
        => this.tripRepository.All()
            .Where(t => t.IsDeleted == false && t.StartDate >= DateTime.UtcNow)
            .Select(t => new TripsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                AvailableSeats = t.AvailableSeats,
                Description = t.Description,
                CreatorName = t.User.UserName,
                StartDate = t.StartDate.ToString("G"),
                CreatorId = t.UserId,
                Members = t.Travellers
                .Select(m => new UserViewModel
                {
                    Id = m.Id,
                    UserName = m.User.UserName,
                    Age = m.User.Age,
                    HomeTown = m.User.HomeTown.Name,
                    CurrentTripId = t.Id,
                    Badges = m.User.Badges
                    .Select(b => new BadgeViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                    })
                    .ToList(),
                })
                .ToList(),
            }).ToList();

        public ICollection<TripsViewModel> GetAllUserTrips(string userId)
        => this.tripRepository.All()
            .Where(t => t.UserId == userId && t.IsDeleted == false)
            .Select(t => new TripsViewModel
            {
                Id = t.Id,
                CreatorId = userId,
                AvailableSeats = t.AvailableSeats,
                CreatorName = t.User.UserName,
                Description = t.Description,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                StartDate = t.StartDate.ToString("G"),
                Name = t.Name,
                Members = t.Travellers
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    UserName = u.User.UserName,
                    Age = u.User.Age,
                    HomeTown = u.User.HomeTown.Name,
                    Badges = u.User.Badges
                    .Select(b => new BadgeViewModel
                    {
                        Name = b.Name,
                        Id = b.Id,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .ToList();

        public ICollection<TripsViewModel> GetDayAfterTrips(string userId)
        => this.tripRepository.All()
            .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear - DateTime.Today.DayOfYear > 1)
            .Select(t => new TripsViewModel
            {
                Name = t.Name,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                CreatorName = t.User.UserName,
                Description = t.Description,
            })
            .ToList();

        public ICollection<TripsViewModel> GetPastTrips(string userId)
        => this.tripRepository.All()
            .Where(t => t.UserId == userId && t.StartDate.Date.DayOfYear.CompareTo(DateTime.Today.DayOfYear) < 0)
            .Select(t => new TripsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                CreatorName = t.User.UserName,
                Description = t.Description,
                Likes = t.Likes.Count,
                Comments = t.Comments,
                Members = t.Travellers
                .Select(m => new UserViewModel
                {
                    UserName = m.User.UserName,
                    Age = m.User.Age,
                    HomeTown = m.User.HomeTown.Name,
                    Badges = m.User.Badges
                    .Select(b => new BadgeViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                    }).ToList(),
                }).ToList(),
            }).ToList();

        public TripsViewModel GetTripById(string tripId, string userId)
        => this.tripRepository.All()
            .Where(t => t.Id == tripId && t.IsDeleted == false)
            .Select(t => new TripsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                AvailableSeats = t.AvailableSeats,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                Description = t.Description,
                StartDate = t.StartDate.ToString("G"),
                CreatorName = t.User.UserName,
                CreatorId = t.UserId,
                CurrentUserId = userId,
                Members = t.Travellers
                .Select(m => new UserViewModel
                {
                    Id = m.Id,
                    UserName = m.User.UserName,
                    Age = m.User.Age,
                    HomeTown = m.User.HomeTown.Name,
                    CurrentTripId = t.Id,
                    Badges = m.User.Badges
                    .Select(b => new BadgeViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .FirstOrDefault();

        public ICollection<TripsViewModel> GetUpcommingTodayTrips(string userId)
        => this.tripRepository.All()
            .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear.CompareTo(DateTime.Today.DayOfYear) == 0)
            .Select(t => new TripsViewModel
            {
                Name = t.Name,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                CreatorName = t.User.UserName,
                Description = t.Description,
            })
            .ToList();

        public ICollection<TripsViewModel> GetUpcommingTomorrowTrips(string userId)
       => this.tripRepository.All()
           .Where(t => t.Travellers.Any(u => u.UserId == userId) && t.IsDeleted == false && t.StartDate.DayOfYear - DateTime.UtcNow.DayOfYear == 1)
           .Select(t => new TripsViewModel
           {
               Name = t.Name,
               FromTown = t.FromTown.Name,
               ToTown = t.ToTown.Name,
               CreatorName = t.User.UserName,
               Description = t.Description,
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
    }
}
