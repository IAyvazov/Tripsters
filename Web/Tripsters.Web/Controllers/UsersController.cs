namespace Tripsters.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Services.Data.Users;
    using Tripsters.Web.ViewModels.Badges;
    using Tripsters.Web.ViewModels.Trips;
    using Tripsters.Web.ViewModels.Users;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Profile(string userId)
        {
            var user = this.usersService.GetUserProfile(this.User.Identity.Name);

            if (userId != null)
            {
                user = this.usersService.GetUserProfileById(userId);
            }

            return this.View(user);
        }

        public IActionResult Edit()
        {
            var user = this.usersService.GetUserProfile(this.User.Identity.Name);

            return this.View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserProfileViewModel userData)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(userData);
            }

            await this.usersService.Edit(userData);

            return this.RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> AddBadge(string badgeId, string userId)
        {
            await this.usersService.AddBadgeToUser(badgeId, userId);

            return this.Redirect("/Trips/Past");
        }
    }
}
