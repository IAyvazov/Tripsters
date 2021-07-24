namespace Tripsters.Web.ViewModels.Photos
{
    public class PhotoViewModel
    {
        public string Url { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int PhotosPerPage { get; } = 4;

        public int TotalPhotos { get; set; }
    }
}
