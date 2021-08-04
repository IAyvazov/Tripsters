namespace Tripsters.Services.Data.Photos
{
    using System.Threading.Tasks;

    public interface IPhotosService
    {
        Task AddPhoto(string path, string userId);

        Task<bool> DeletePhoto(int photoId, string userId);

        // Task<int> LikePhoto(int photoId, string userId);
    }
}
