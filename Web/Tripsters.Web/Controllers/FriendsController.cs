namespace Tripsters.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Common;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Notifications;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;

    public class FriendsController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INotificationsService notificationsService;
        private readonly IUsersService usersService;

        public FriendsController(
            UserManager<ApplicationUser> userManager,
            INotificationsService notificationsService,
            IUsersService usersService)
        {
            this.usersService = usersService;
            this.userManager = userManager;
            this.notificationsService = notificationsService;
        }

        [Authorize]
        public IActionResult MyFriends(UserProfileServiceModel userModel)
        {
            var user = this.usersService.GetUserProfile(this.User.Identity.Name);

            if (userModel.UserId != null)
            {
                user = this.usersService.GetUserProfileById(userModel.UserId, userModel.CurrentPage, userModel.PhotosPerPage);
            }

            return this.View(user);
        }

        [Authorize]
        public async Task<IActionResult> Add(string friendUserId)
        {
            var currUserId = this.userManager.GetUserId(this.User);

            await this.notificationsService.Notifie(currUserId, friendUserId, GlobalConstants.NotifeFriendRequestText);

            return this.Redirect($"/Users/Profile?userId={friendUserId}");
        }

        public async Task<IActionResult> Confirm(string currUserId, string friendUserId, int notificationId)
        {
            await this.usersService.AddFriend(currUserId, friendUserId);
            await this.notificationsService.Seen(notificationId);

            return this.Redirect($"/Users/Profile?userId={friendUserId}");
        }
    }
}
