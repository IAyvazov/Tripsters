namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        Task AddTrip(TripsInputFormModel tripData, string userName);

        Task EditTrip(TripsViewModel tripData);

        ICollection<TripsViewModel> GetAllTrips(int currentPage, int tripsPerPage);

        int GetAllTripsCount();

        int GetAllUserTripsCount(string userId);

        ICollection<TripsViewModel> GetAllUserTrips(string userId, int currentPage, int tripsPerPage);

        TripsViewModel GetTripById(string tripId, string userId);

        Task JoinTrip(string tripId, string userId);

        Task Delete(string tripId);

        ICollection<TripsViewModel> GetUpcommingTodayTrips(string userId);

        ICollection<TripsViewModel> GetUpcommingTomorrowTrips(string userId);

        ICollection<TripsViewModel> GetPastTrips(string userId);

        ICollection<TripsViewModel> GetDayAfterTrips(string userId);

        Task<int> LikeTrip(string tripId, string userId);

        Task AddComment(string userId, string tripId, string commentInput);

        public ICollection<CommentViewModel> GetAllTripComments(string tripId);
    }
}
