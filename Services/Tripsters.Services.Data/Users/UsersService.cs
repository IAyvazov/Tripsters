namespace Tripsters.Services.Data.Users
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Photos;
    using Tripsters.Web.ViewModels.Users;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepostory;
        private readonly IDeletableEntityRepository<UsersBadges> userBadgesRepostory;
        private readonly IBadgesService badgesService;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> userRepostory,
            IDeletableEntityRepository<UsersBadges> userBadgesRepostory,
            IBadgesService badgesService)
        {
            this.userRepostory = userRepostory;
            this.userBadgesRepostory = userBadgesRepostory;
            this.badgesService = badgesService;
        }

        public async Task AddBadgeToUser(string badgeId, string userId)
        {
            var user = this.userRepostory.All()
                .Where(u => u.Id == userId)
                .Include(x => x.Badges)
                .FirstOrDefault();

            if (user == null)
            {
                throw new ArgumentNullException("There is no such user.");
            }

            var badge = this.badgesService.GetBadgeById(badgeId);

            if (badge == null)
            {
                throw new ArgumentNullException("There is no such badge");
            }

            if (user.Badges.Any(b => b.BadgeId == badgeId))
            {
                return;
            }

            var userBadges = new UsersBadges { BadgeId = badgeId, UserId = userId };

            await this.userBadgesRepostory.AddAsync(userBadges);

            await this.userBadgesRepostory.SaveChangesAsync();
        }

        public async Task AddPhoto(string path, string userId)
        {
            var user = this.userRepostory.All()
               .Where(u => u.Id == userId)
               .FirstOrDefault();

            var photo = new Photo { Url = path.Substring(62), IsProfilePicture = false };

            user.Photos.Add(photo);

            await this.userRepostory.SaveChangesAsync();
        }

        public async Task Edit(UserProfileViewModel userData)
        {
            var user = this.userRepostory.All()
                .Where(u => u.Id == userData.Id)
                .FirstOrDefault();

            user.UserName = userData.UserName;
            user.Age = userData.Age;
            user.Email = userData.Email;

            user.Photos.Add(new Photo { Url = userData.ProfilePictureUrl.Substring(62), UserId = user.Id, IsProfilePicture = true });

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
                    Id = b.Badge.Id,
                    Name = b.Badge.Name,
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
                    Id = b.Badge.Id,
                    Name = b.Badge.Name,
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
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                    })
                    .ToList(),
                })
                .ToList(),
                Photos = u.Photos
                .Where(p => p.IsProfilePicture == false)
                .Select(p => new PhotoViewModel
                {
                    Url = p.Url,
                })
                .ToList(),
            })
            .FirstOrDefault();

        public UserProfileViewModel GetUserProfileById(string userId)
         => this.userRepostory.All()
            .Where(u => u.Id == userId)
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
                    Id = b.Badge.Id,
                    Name = b.Badge.Name,
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
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                    })
                    .ToList(),
                })
                .ToList(),
                Photos = u.Photos
                .Where(p => p.IsProfilePicture == false)
                .Select(p => new PhotoViewModel
                {
                    Url = p.Url,
                })
                .ToList(),
            })
            .FirstOrDefault();
    }
}
