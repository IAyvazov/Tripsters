namespace Tripsters.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Landmark : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public int TownId { get; set; }

        public Town Town { get; set; }

        public string Description { get; set; }
    }
}