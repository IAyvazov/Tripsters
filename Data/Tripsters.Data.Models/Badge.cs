namespace Tripsters.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Badge : BaseDeletableModel<string>
    {
        public Badge()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Users = new HashSet<UsersBadges>();
        }

        [Required]
        public string Name { get; set; }

        public ICollection<UsersBadges> Users { get; set; }

        // Funny,
        // Adventurer,
        // Grumbler,
        // Talkative,
        // Angry,
        // Crazy,
    }
}
