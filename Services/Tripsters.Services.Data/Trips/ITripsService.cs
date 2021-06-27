namespace Tripsters.Services.Data.Trips
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Tripsters.Web.ViewModels.Trips;

    public interface ITripsService
    {
        Task Add(TripsInputFormModel tripData);

        ICollection<T> GetUserTrips<T>(string userId);
    }
}
