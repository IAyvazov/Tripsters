namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Towns;
    using Tripsters.Web.ViewModels.Trips;

    using static Tripsters.Common.GlobalConstants;

    public class TripsService : ITripsService
    {
        private readonly IDeletableEntityRepository<Trip> tripRepository;
        private readonly ITownsService townsService;

        public TripsService(IDeletableEntityRepository<Trip> tripRepository, ITownsService townsService)
        {
            this.tripRepository = tripRepository;
            this.townsService = townsService;
        }

        public async Task AddTrip(TripsInputFormModel tripData)
        {
            var trip = new Trip
            {
                AvailableSeats = tripData.AvailableSeats,
                Name = tripData.Name,
                Description = tripData.Description,
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

            trip.FromTown = fromTown;
            trip.ToTown = toTown;

            await this.tripRepository.AddAsync(trip);
            await this.tripRepository.SaveChangesAsync();
        }

        public ICollection<TripsViewModel> GetAllTrips()
        => this.tripRepository.All()
            .Select(t => new TripsViewModel
            {
                Name = t.Name,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                AvailableSeats = t.AvailableSeats,
                Description = t.Description,
            })
            .ToList();

        public TripsViewModel GetTripById(string tripId)
        => this.tripRepository.All()
            .Where(t => t.Id == tripId)
            .Select(t => new TripsViewModel
            {
                Name = t.Name,
                AvailableSeats = t.AvailableSeats,
                FromTown = t.FromTown.Name,
                ToTown = t.ToTown.Name,
                Description = t.Description,

            })
            .FirstOrDefault();

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
