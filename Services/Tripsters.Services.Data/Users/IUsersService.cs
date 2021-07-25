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

        UserProfileServiceModel GetUserProfileById(string userId, int currentPage, int photosPerPage);

        Task Edit(UserProfileServiceModel userData);

        Task AddBadgeToUser(int badgeId, string userId, string userWhoAddId);

        Task AddPhoto(string path, string userId);

        Task<bool> DeletePhoto(int photoId, string userId);

        Task AddFriend(string currUserId, string friendUserId);
    }
}
