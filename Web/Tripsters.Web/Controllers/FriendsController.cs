namespace Tripsters.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;

    public class FriendsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;

        public FriendsController(IUsersService usersService, UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.userManager = userManager;
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
            await this.usersService.AddFriend(currUserId, friendUserId);

            return this.Redirect($"/Users/Profile?userId={friendUserId}");
        }

    }
}
