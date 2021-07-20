namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        Task AddTrip(TripsInputFormModel tripData, string userName);

        Task EditTrip(TripServiceModel tripData);

        ICollection<TripServiceModel> GetAllTrips(int currentPage, int tripsPerPage);

        int GetAllTripsCount();

        int GetAllUserTripsCount(string userId);

        ICollection<TripServiceModel> GetAllUserTrips(string userId, int currentPage, int tripsPerPage);

        TripServiceModel GetTripById(string tripId, string userId);

        Task JoinTrip(string tripId, string userId);

        Task Delete(string tripId);

        ICollection<TripServiceModel> GetUpcommingTodayTrips(string userId);

        ICollection<TripServiceModel> GetUpcommingTomorrowTrips(string userId);

        ICollection<TripServiceModel> GetPastTrips(string userId, int currentPage, int tripsPerPage);

        ICollection<TripServiceModel> GetDayAfterTrips(string userId);

        Task<int> LikeTrip(string tripId, string userId);

        Task AddComment(string userId, string tripId, string commentInput);

        public ICollection<CommentViewModel> GetAllTripComments(string tripId);
    }
}
