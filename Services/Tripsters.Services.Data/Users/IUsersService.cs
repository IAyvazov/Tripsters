namespace Tripsters.Services.Data.Users
{
    using Tripsters.Data.Models;
    using Tripsters.Web.ViewModels.Users;

    public interface IUsersService
    {
        ApplicationUser GetUser(string userName);

        UserViewModel GetUserById(string creatorId, string userId, string currTripId);
    }
}
