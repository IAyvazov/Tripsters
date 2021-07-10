namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        Task AddTrip(TripsInputFormModel tripData, string userName);

        Task EditTrip(TripsViewModel tripData);

        ICollection<TripsViewModel> GetAllTrips();

        ICollection<TripsViewModel> GetAllUserTrips(string userId);

        TripsViewModel GetTripById(string tripId, string userId);

        Task JoinTrip(string tripId, string userId);

        Task Delete(string tripId);

        ICollection<TripsViewModel> GetUpcommingTodayTrips(string userId);

        ICollection<TripsViewModel> GetUpcommingTomorrowTrips(string userId);

        ICollection<TripsViewModel> GetPastTrips(string userId);

        ICollection<TripsViewModel> GetDayAfterTrips(string userId);

        Task<int> LikeTrip(string tripId, string userId);
    }
}
