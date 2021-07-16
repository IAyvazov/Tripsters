namespace Tripsters.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;
    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepostory;

        public UsersService(IDeletableEntityRepository<ApplicationUser> userRepostory)
        {
            this.userRepostory = userRepostory;
        }

        public async Task Edit(UserProfileViewModel userData)
        {
            var user = this.userRepostory.All()
                .Where(u => u.Id == userData.Id)
                .FirstOrDefault();

            user.UserName = userData.UserName;
            user.Age = userData.Age;
            user.Email = userData.Email;
            user.Photos.Add(new Photo { Url = userData.ProfilePictureUrl, UserId = user.Id, IsProfilePicture = true });

            await this.userRepostory.SaveChangesAsync();
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

        public UserProfileViewModel GetUserProfile(string userName)
        => this.userRepostory.All()
            .Where(u => u.UserName == userName)
            .Select(u => new UserProfileViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Age = u.Age,
                HomeTown = u.HomeTown.Name,
                Email = u.Email,
                ProfilePictureUrl = u.Photos
                .Where(p => p.IsProfilePicture == true)
                .FirstOrDefault().Url,
                Badges = u.Badges
                .Select(b => new BadgeViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                })
                .ToList(),
                Friends = u.Friends
                .Select(f => new UserViewModel
                {
                    Id = f.Id,
                    UserName = f.UserName,
                    Age = f.Age,
                    HomeTown = f.HomeTown.Name,
                    Badges = f.Badges.Select(b => new BadgeViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                    })
                    .ToList(),
                })
                .ToList(),
            })
            .FirstOrDefault();
    }
}
