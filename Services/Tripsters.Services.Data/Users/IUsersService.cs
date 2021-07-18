namespace Tripsters.Services.Data.Users
{
    using System.Threading.Tasks;

    using Tripsters.Data.Models;
    using Tripsters.Web.ViewModels.Users;

    public interface IUsersService
    {
        ApplicationUser GetUser(string userName);

        UserViewModel GetUserById(string creatorId, string userId, string currTripId);

        UserProfileViewModel GetUserProfile(string userName);

        UserProfileViewModel GetUserProfileById(string userId);

        Task Edit(UserProfileViewModel userData);

        Task AddBadgeToUser(string badgeId, string userId);
    }
}
