namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Towns;
    using Tripsters.Web.ViewModels.Trips;

    public class TripsService : ITripsService
    {
        private readonly IDeletableEntityRepository<Trip> tripsRepository;
        private readonly ITownService townService;

        public TripsService(IDeletableEntityRepository<Trip> tripsRepository, ITownService townService)
        {
            this.tripsRepository = tripsRepository;
            this.townService = townService;
        }

        public async Task Add(TripsInputFormModel tripData)
        {
            var fromTown = this.townService.GetTownByName(tripData.FromTown);

            if (fromTown == null)
            {
                fromTown = await this.townService
                    .AddTown(new Town { Name = tripData.FromTown });
            }

            var toTown = this.townService.GetTownByName(tripData.ToTown);

            if (toTown == null)
            {
                toTown = await this.townService
                    .AddTown(new Town { Name = tripData.ToTown });
            }

            var trip = new Trip
            {
                Name = tripData.Name,
                AvailableSeats = tripData.AvailableSeats,
                FromTownId = fromTown.Id,
                ToTownId = toTown.Id,
                Description = tripData.Description,
            };

            await this.tripsRepository.AddAsync(trip);
            await this.tripsRepository.SaveChangesAsync();
        }

        public ICollection<T> GetUserTrips<T>(string userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
