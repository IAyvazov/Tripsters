namespace Tripsters.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Tripsters.Services.Data.Users;
    using Tripsters.Web.ViewModels.Users;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult Profile()
        {
            var user = this.usersService.GetUserProfile(this.User.Identity.Name);

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
    }
}
