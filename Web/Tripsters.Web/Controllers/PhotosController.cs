namespace Tripsters.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Data.Models;
    using Tripsters.Services.Data.Notifications;
    using Tripsters.Services.Data.Photos;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;

    using static Tripsters.Common.GlobalConstants;

    public class PhotosController : BaseController
    {
        private readonly INotificationsService notificationsService;
        private readonly IUsersService usersService;
        private readonly IPhotosService photosService;
        private readonly IWebHostEnvironment environment;

        public PhotosController(
            UserManager<ApplicationUser> userManager,
            INotificationsService notificationsService,
            IUsersService usersService,
            IPhotosService photosService,
            IWebHostEnvironment environment)
        {
            this.usersService = usersService;
            this.photosService = photosService;
            this.notificationsService = notificationsService;
            this.environment = environment;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(List<IFormFile> postedFiles, string userId)
        {
            string path = Path.Combine(this.environment.WebRootPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                using FileStream stream = new(Path.Combine(path, fileName), FileMode.Create);
                postedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                this.ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                await this.photosService.AddPhoto(path + $"\\{fileName}", userId);
            }

            this.TempData[GlobalMessageKey] = "Your photo was successfully uploaded!";

            return this.Redirect("/Users/Profile");
        }

        [Authorize]
        public async Task<IActionResult> Delete(int photoId, string userId, int currentPage, int photosPerPage)
        {
            var isDeleted = await this.photosService.DeletePhoto(photoId, userId);

            if (!isDeleted)
            {
                return this.BadRequest();
            }

            this.TempData[GlobalMessageKey] = "Your photo was successfully deleted!";

            return this.Redirect($"/Photos/All?userId={userId}&currentPage={currentPage}&photosPerPage={photosPerPage}");
        }

        [Authorize]
        public IActionResult All(UserProfileServiceModel userModel)
        {
            var user = this.usersService.GetUserProfileById(userModel.UserId, userModel.CurrentPage, userModel.PhotosPerPage);
            return this.View(user);
        }

        [Authorize]
        public async Task<IActionResult> Like(int photoId, string currUserId, string userId)
        {
            await this.photosService.Like(photoId, currUserId);

            await this.notificationsService.Notifie(currUserId, userId, Notifications.PhotoLikeText);

            return this.RedirectToAction(nameof(this.All), new { UserId = userId });
        }
    }
}
