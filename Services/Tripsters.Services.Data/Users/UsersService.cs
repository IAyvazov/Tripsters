namespace Tripsters.Services.Data.Users
{
    using System.Linq;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Trips;
    using Tripsters.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepostory;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepostory)
        {
            this.userRepostory = userRepostory;
        }

        public ApplicationUser GetUser(string userName)
        => this.userRepostory.All()
            .Where(u => u.UserName == userName)
            .FirstOrDefault();

        public UserViewModel GetUserById(string creatorId, string userId, string currTripId)
        => this.userRepostory.All()
            .Where(u => u.Id == creatorId)
            .Select(u => new UserViewModel
            {
                Id = creatorId,
                Age = u.Age,
                UserName = u.UserName,
                HomeTown = u.HomeTown.Name,
                CurrentTripId = currTripId,
                Badges = u.Badges
                .Select(b => new BadgeViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                }).ToList(),
                MutualFriends = u.Friends.Where(x => x.Id == creatorId && x.Id == userId)
                .Select(f => new UserViewModel
                {
                    UserName = f.UserName,
                    Age = f.Age,
                    HomeTown = f.HomeTown.Name,
                }).ToList(),
            }).FirstOrDefault();
    }
}
