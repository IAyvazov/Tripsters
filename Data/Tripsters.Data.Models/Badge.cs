namespace Tripsters.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Badge : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

        //Funny,
        //Adventurer,
        //Grumbler,
        //Talkative,
        //Angry,
        //Crazy,
    }
}
