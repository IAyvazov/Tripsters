namespace Tripsters.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Badge : BaseDeletableModel<int>
    {
        public Badge()
        {
            this.Users = new HashSet<UsersBadges>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<UsersBadges> Users { get; set; }
    }
}
