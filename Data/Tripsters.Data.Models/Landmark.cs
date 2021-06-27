namespace Tripsters.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Tripsters.Data.Common.Models;

    public class Landmark : BaseDeletableModel<string>
    {
        public Landmark()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(40)]
        public string Name { get; set; }

        public string ImagePath { get; set; }

        public string TownId { get; set; }

        public Town Town { get; set; }

        public string Description { get; set; }
    }
}