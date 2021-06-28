namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        ICollection<string> Validate(TripsInputFormModel tripData);

        Task AddTrip(TripsInputFormModel tripData);

        ICollection<TripsViewModel> GetAllTrips();

        TripsViewModel GetTripById(string tripId);
    }
}
