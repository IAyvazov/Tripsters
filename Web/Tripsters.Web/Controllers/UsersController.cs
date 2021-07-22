namespace Tripsters.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;

    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private IWebHostEnvironment environment;


        public UsersController(
            IUsersService usersService,
            IWebHostEnvironment environment,
            UserManager<ApplicationUser> userManager)
        {
            this.usersService = usersService;
            this.environment = environment;
            this.userManager = userManager;
        }

        public async Task<IActionResult> AddFriend(string friendUserId)
        {
            var currUserId = this.userManager.GetUserId(this.User);
            await this.usersService.AddFriend(currUserId, friendUserId);

            return this.Redirect($"/Users/Profile?userId={friendUserId}");
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
        public async Task<IActionResult> Edit(UserProfileServiceModel userData)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(userData);
            }

            string path = Path.Combine(this.environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = Path.GetFileName(userData.ProfilePicture.FileName);
            using FileStream stream = new(Path.Combine(path, fileName), FileMode.Create);
            userData.ProfilePicture.CopyTo(stream);
            this.ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);

            userData.ProfilePictureUrl = path + $"\\{fileName}";

            await this.usersService.Edit(userData);

            return this.RedirectToAction("Profile");
        }

        [HttpPost]
        public async Task<IActionResult> AddBadge(int badgeId, string userId)
        {
            await this.usersService.AddBadgeToUser(badgeId, userId);

            return this.Redirect("/Trips/Past");
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(List<IFormFile> postedFiles, string userId)
        {
            string path = Path.Combine(this.environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using FileStream stream = new(Path.Combine(path, fileName), FileMode.Create);
                postedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                this.ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                await this.usersService.AddPhoto(path + $"\\{fileName}", userId);
            }

            return this.RedirectToAction("Profile");
        }

        public IActionResult AllPhoto(string userId)
        {
            var user = this.usersService.GetUserProfileById(userId);

            return this.View(user);
        }
    }
}
