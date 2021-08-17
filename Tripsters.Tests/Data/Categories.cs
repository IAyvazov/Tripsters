namespace Tripsters.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Tripsters.Services.Data.Trips.Models;

    public static class Categories
    {
        public static IEnumerable<TripCategoryServiceModel> ThreeCategories
           => Enumerable.Range(0, 3).Select(i => new TripCategoryServiceModel
           {
               Name = $"name{i}",
               Id = 1 + i,
           });
    }
}
