namespace Tripsters.Web.Controllers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Tripsters.Services.Data.Photos;
    using Tripsters.Services.Data.Users;
    using Tripsters.Services.Data.Users.Models;

    public class PhotosController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPhotosService photosService;
        private readonly IWebHostEnvironment environment;

        public PhotosController(IWebHostEnvironment environment, IPhotosService photosService, IUsersService usersService)
        {
            this.environment = environment;
            this.photosService = photosService;
            this.usersService = usersService;
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

            return this.Redirect($"/Photos/All?userId={userId}&currentPage={currentPage}&photosPerPage={photosPerPage}");
        }

        [Authorize]
        public IActionResult All(UserProfileServiceModel userModel)
        {
            var user = this.usersService.GetUserProfileById(userModel.UserId, userModel.CurrentPage, userModel.PhotosPerPage);
            return this.View(user);
        }
    }
}
