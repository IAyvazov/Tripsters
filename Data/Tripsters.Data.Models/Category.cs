namespace Tripsters.Data.Models
{
    using System.Collections.Generic;

    using Tripsters.Data.Common.Models;

    public class Category : BaseDeletableModel<int>
    {
        public Category()
        {
            this.Trips = new HashSet<Trip>();
        }

        public string Name { get; set; }

        public ICollection<Trip> Trips { get; set; }
    }
}
