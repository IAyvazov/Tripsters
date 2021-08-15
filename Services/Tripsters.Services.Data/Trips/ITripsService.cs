namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Services.Data.Trips.Models;
    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        IEnumerable<TripServiceModel> GetAllTripsForAdmin(int currentPage, int tripsPerPage);

        IEnumerable<TripCategoryServiceModel> AllCategories();

        Task AddTrip(TripServiceFormModel tripData, string userName);

        Task EditTrip(string tripId, TripServiceFormModel tripData);

        IEnumerable<TripServiceModel> GetAllTrips(int currentPage, int tripsPerPage);

        int GetAllTripsCount();

        int GetAllUserTripsCount(string userId);

        IEnumerable<TripServiceModel> GetAllUserTrips(string userId, int currentPage, int tripsPerPage);

        TripServiceModel GetTripById(string tripId, string userId);

        Task JoinTrip(string tripId, string userId);

        Task Delete(string tripId);

        IEnumerable<TripServiceModel> GetUpcommingTodayTrips(string userId);

        IEnumerable<TripServiceModel> GetUpcommingTomorrowTrips(string userId);

        IEnumerable<TripServiceModel> GetPastTrips(string userId, int currentPage, int tripsPerPage);

        Task<int> LikeTrip(string tripId, string userId);

        Task AddComment(string userId, string tripId, string text);

        IEnumerable<CommentViewModel> GetAllTripComments(string tripId);

        IEnumerable<TripServiceModel> RecentTrips(string userId, int currentPage, int tripsPerPage);

        IEnumerable<TripServiceModel> GetAllTripsByCategoryId(int categoryId, int currentPage, int tripsPerPage);

        Task<string> Approve(string tripId);
    }
}
