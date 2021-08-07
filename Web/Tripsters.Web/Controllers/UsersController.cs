﻿namespace Tripsters.Web.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Tripsters.Common;
    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Notifications;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;

    public class UsersController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly INotificationsService notificationsService;
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment environment;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            INotificationsService notificationsService,
            IUsersService usersService,
            IWebHostEnvironment environment)
        {
            this.usersService = usersService;
            this.notificationsService = notificationsService;
            this.environment = environment;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Profile(UserProfileServiceModel userModel)
        {
            var user = this.usersService.GetUserProfile(this.User.Identity.Name);

            if (userModel.UserId != null)
            {
                user = this.usersService.GetUserProfileById(userModel.UserId, userModel.CurrentPage, userModel.PhotosPerPage);
            }

            return this.View(user);
        }

        [Authorize]
        public IActionResult Edit()
        {
            var user = this.usersService.GetUserProfile(this.User.Identity.Name);

            return this.View(user);
        }

        [HttpPost]
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> AddBadge(int badgeId, string userId, string currUserId)
        {
            await this.usersService.AddBadgeToUser(badgeId, userId, currUserId);

            await this.notificationsService.Notifie(currUserId, userId, GlobalConstants.NotifeBadgeText);

            return this.Redirect($"/Users/Profile?userId={userId}");
        }
    }
}
