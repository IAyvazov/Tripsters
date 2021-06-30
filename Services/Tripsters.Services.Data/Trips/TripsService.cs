namespace Tripsters.Services.Data.Trips
{
    using System;
    using System.Collections.Generic;
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
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly ITownsService townsService;
        private readonly IUsersService usersService;

        public TripsService(
            IDeletableEntityRepository<Trip> tripRepository,
            IDeletableEntityRepository<ApplicationUser> userRepository,
            ITownsService townsService,
            IUsersService usersService)
        {
            this.tripRepository = tripRepository;
            this.userRepository = userRepository;
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
                StartDate = DateTime.UtcNow,
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

        public ICollection<TripsViewModel> GetAllTrips()
        => this.tripRepository.All()
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
                Members = t.Travellers
                .Select(m => new UserViewModel
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    Age = m.Age,
                    HomeTown = m.HomeTown.Name,
                    CurrentTripId = t.Id,
                    Badges = m.Badges
                    .Select(b => new BadgeViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                    })
                    .ToList(),
                })
                .ToList(),
            }).ToList();

        public TripsViewModel GetTripById(string tripId, string userId)
        => this.tripRepository.All()
            .Where(t => t.Id == tripId)
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
                    UserName = m.UserName,
                    Age = m.Age,
                    HomeTown = m.HomeTown.Name,
                    CurrentTripId = t.Id,
                    Badges = m.Badges
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

        public async Task JoinTrip(string tripId, string userId)
        {
            var user = this.userRepository.All()
                .FirstOrDefault(u => u.Id == userId);

            var trip = this.tripRepository.All()
                  .FirstOrDefault(t => t.Id == tripId);

            trip.AvailableSeats--;
            trip.Travellers.Add(user);

            await this.tripRepository.SaveChangesAsync();
        }

        public ICollection<string> Validate(TripsInputFormModel tripData)
        {
            var errors = new HashSet<string>();

            if (tripData.Name == null || tripData.Name.Length < TripSecurity.NameMinLength || tripData.Name.Length > TripSecurity.NameMaxLength)
            {
                errors.Add($"The Name must be at least {TripSecurity.NameMinLength} and at max {TripSecurity.NameMaxLength} characters long.");
            }

            if (tripData.FromTown == null || tripData.FromTown.Length < TripSecurity.TownMinLength || tripData.FromTown.Length > TripSecurity.TownMinLength)
            {
                errors.Add($"The Town must be at least {TripSecurity.TownMinLength} and at max {TripSecurity.TownMaxLength} characters long.");
            }

            if (tripData.ToTown == null || tripData.ToTown.Length < TripSecurity.TownMinLength || tripData.ToTown.Length > TripSecurity.TownMinLength)
            {
                errors.Add($"The Town must be at least {TripSecurity.TownMinLength} and at max {TripSecurity.TownMaxLength} characters long.");
            }

            if (tripData.AvailableSeats < 1 || tripData.AvailableSeats > 6)
            {
                errors.Add($"The Available seats must be at least 1 and at max 6 characters long.");
            }


            if (tripData.Description.Length < TripSecurity.DescriptionMinLength || tripData.Description.Length > TripSecurity.DescriptionMaxLength)
            {
                errors.Add($"The Description seats must be at least {TripSecurity.DescriptionMinLength} and at max {TripSecurity.DescriptionMaxLength} characters long.");
            }

            return errors;
        }
    }
}
