namespace Tripsters.Services.Data.Users
{
    using System.Threading.Tasks;

    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Users.Models;

    public interface IUsersService
    {
        ApplicationUser GetUser(string userId);

        UserServiceModel GetUserById(string creatorId, string userId, string currTripId);

        UserProfileServiceModel GetUserProfile(string userName);

        UserProfileServiceModel GetUserProfileById(string userId);

        Task Edit(UserProfileServiceModel userData);

        Task AddBadgeToUser(int badgeId, string userId);

        Task AddPhoto(string path, string userId);
    }
}
