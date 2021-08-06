namespace Tripsters.Services.Data.Photos
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Tripsters.Data;
    using Tripsters.Data.Models;

    public class PhotosService : IPhotosService
    {
        private readonly ApplicationDbContext dbContext;

        public PhotosService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> Like(int photoId, string userId)
        {
            var photo = this.dbContext.Photos
                .Where(t => t.Likes.Any(l => l.PhotoId == photoId && l.UserId == userId))
                .FirstOrDefault();

            if (photo == null)
            {
                var like = new Like
                {
                    PhotoId = photoId,
                    UserId = userId,
                    TripId = null,
                };

                var currentPhoto = this.dbContext.Photos
                           .Where(t => t.Id == photoId)
                           .FirstOrDefault();

                currentPhoto.Likes.Add(like);

                await this.dbContext.Likes.AddAsync(like);

                await this.dbContext.SaveChangesAsync();

                return currentPhoto.Likes.Count;
            }

            return photo.Likes.Count;
        }

        public async Task AddPhoto(string path, string userId)
        {
            var user = this.dbContext.Users
               .Where(u => u.Id == userId)
               .FirstOrDefault();

            var photo = new Photo { Url = path.Substring(62), IsProfilePicture = false };

            user.Photos.Add(photo);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeletePhoto(int photoId, string userId)
        {
            var user = this.dbContext.Users
                .Where(u => u.Id == userId)
                .Include(x => x.Photos)
                .FirstOrDefault();

            if (user == null)
            {
                return false;
            }

            var photo = user.Photos
                .FirstOrDefault(p => p.IsDeleted == false && p.Id == photoId);

            if (photo == null)
            {
                return false;
            }

            photo.IsDeleted = true;

            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}
