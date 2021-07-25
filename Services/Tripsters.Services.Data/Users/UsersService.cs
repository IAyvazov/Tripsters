namespace Tripsters.Services.Data.Users
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Tripsters.Data.Common.Repositories;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Badges;
    using Tripsters.Services.Data.Users.Models;

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

        public async Task AddBadgeToUser(int badgeId, string userId, string userWhoAddId)
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

            var userBadges = new UsersBadges { BadgeId = badgeId, UserId = userId, AdderId = userWhoAddId };

            await this.userBadgesRepostory.AddAsync(userBadges);

            await this.userBadgesRepostory.SaveChangesAsync();
        }

        public async Task AddFriend(string currUserId, string friendUserId)
        {
            var user = this.GetUser(currUserId);
            var friend = this.GetUser(friendUserId);

            var userFriends = new UserFriend { UserId = currUserId, FriendId = friendUserId };
            var friendFriends = new UserFriend { UserId = friendUserId, FriendId = currUserId };

            friend.Friends.Add(friendFriends);
            user.Friends.Add(userFriends);
            await this.userRepostory.SaveChangesAsync();
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

        public async Task Edit(UserProfileServiceModel userData)
        {
            var user = this.userRepostory.All()
                .Where(u => u.Id == userData.UserId)
                .FirstOrDefault();

            user.UserName = userData.UserName;
            user.Age = userData.Age;
            user.Email = userData.Email;
            user.Photos.Add(new Photo { Url = userData.ProfilePictureUrl.Substring(62), UserId = user.Id, IsProfilePicture = true });

            await this.userRepostory.SaveChangesAsync();
        }

        public ApplicationUser GetUser(string userId)
        => this.userRepostory.All()
            .Where(u => u.Id == userId)
            .Include(p => p.Photos)
            .FirstOrDefault();

        public UserServiceModel GetUserById(string creatorId, string userId, string currTripId)
        => this.userRepostory.All()
            .Where(u => u.Id == creatorId)
            .Select(u => new UserServiceModel
            {
                Id = creatorId,
                Age = u.Age,
                UserName = u.UserName,
                CurrentTripId = currTripId,
                Badges = u.Badges
                .Select(b => new BadgeServiceModel
                {
                    Id = b.Badge.Id,
                    Name = b.Badge.Name,
                    AdderId = b.AdderId,
                }).ToList(),
            }).FirstOrDefault();

        public UserProfileServiceModel GetUserProfile(string userName)
        => this.userRepostory.All()
            .Where(u => u.UserName == userName)
            .Include(f => f.Friends)
            .Select(u => new UserProfileServiceModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                Age = u.Age,
                Email = u.Email,
                TotalPhotos = u.Photos
                .Where(p => p.IsProfilePicture == false)
                .Count(),
                ProfilePictureUrl = u.Photos
                .Where(p => p.IsProfilePicture == true)
                .FirstOrDefault().Url,
                UserBadges = u.Badges
                .Select(b => new BadgeServiceModel
                {
                    Id = b.Badge.Id,
                    Name = b.Badge.Name,
                    AdderId = b.AdderId,
                })
                .ToList(),
                Friends = u.Friends
                .Select(f => new UserServiceModel
                {
                    Id = f.FriendId,
                    UserName = f.Friend.UserName,
                    Age = f.Friend.Age,
                    ProfilePictureUrl = f.Friend.Photos
                    .Where(p => p.IsProfilePicture == true)
                    .FirstOrDefault().Url,
                    Badges = f.Friend.Badges.Select(b => new BadgeServiceModel
                    {
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                        AdderId = b.AdderId,
                    })
                    .ToList(),
                })
                .ToList(),
                Photos = u.Photos
                .Where(p => p.IsProfilePicture == false)
                .Select(p => new PhotoServiceModel
                {
                    Url = p.Url,
                })
                .ToList(),
                AllBadges = this.badgesService.GetAllBadges(),
            })
            .FirstOrDefault();

        public UserProfileServiceModel GetUserProfileById(string userId, int currentPage, int photosPerPage)
         => this.userRepostory.All()
            .Where(u => u.Id == userId)
            .Include(f => f.Friends)
            .Select(u => new UserProfileServiceModel
            {
                UserId = u.Id,
                UserName = u.UserName,
                Age = u.Age,
                Email = u.Email,
                TotalPhotos = u.Photos
                .Where(p => p.IsProfilePicture == false)
                .Count(),
                CurrentPage = currentPage,
                ProfilePictureUrl = u.Photos
                .Where(p => p.IsProfilePicture == true)
                .FirstOrDefault().Url,
                UserBadges = u.Badges
                .Select(b => new BadgeServiceModel
                {
                    Id = b.Badge.Id,
                    Name = b.Badge.Name,
                    AdderId = b.AdderId,
                })
                .ToList(),
                Friends = u.Friends
                .Select(f => new UserServiceModel
                {
                    Id = f.FriendId,
                    UserName = f.Friend.UserName,
                    Age = f.Friend.Age,
                    ProfilePictureUrl = f.Friend.Photos
                    .Where(p => p.IsProfilePicture == true)
                    .FirstOrDefault().Url,
                    Badges = f.Friend.Badges.Select(b => new BadgeServiceModel
                    {
                        Id = b.Badge.Id,
                        Name = b.Badge.Name,
                        AdderId = b.AdderId,
                    })
                    .ToList(),
                })
                .ToList(),
                Photos = u.Photos
                .Where(p => p.IsProfilePicture == false)
                .Skip((currentPage - 1) * photosPerPage)
                .Take(photosPerPage)
                .Select(p => new PhotoServiceModel
                {
                    Url = p.Url,
                })
                .ToList(),
                AllBadges = this.badgesService.GetAllBadges(),
            })
            .FirstOrDefault();
    }
}
