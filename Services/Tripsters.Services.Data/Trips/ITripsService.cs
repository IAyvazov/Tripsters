namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        ICollection<string> Validate(TripsInputFormModel tripData);

        Task AddTrip(TripsInputFormModel tripData, string userName);

        ICollection<TripsViewModel> GetAllTrips();

        ICollection<TripsViewModel> GetAllUserTrips(string userId);

        TripsViewModel GetTripById(string tripId, string userId);

        Task JoinTrip(string tripId, string userId);

        bool IsUserJoined(string tripId, string userId);
    }
}
