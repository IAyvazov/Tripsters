namespace Tripsters.Services.Data.Photos
{
    using System.Collections.Generic;

    using Tripsters.Data.Models;

    public class PhotoServiceModel
    {
        public PhotoServiceModel()
        {
            this.Likes = new HashSet<LikeServiceModel>();
        }

        public int Id { get; set; }

        public string Url { get; set; }

        public ICollection<LikeServiceModel> Likes { get; set; }
    }
}
