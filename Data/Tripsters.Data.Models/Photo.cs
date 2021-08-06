namespace Tripsters.Data.Models
{
    using System.Collections.Generic;

    using Tripsters.Data.Common.Models;

    public class Photo : BaseDeletableModel<int>
    {
        public Photo()
        {
            this.Likes = new HashSet<Like>();
        }

        public string Url { get; set; }

        public bool IsProfilePicture { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
