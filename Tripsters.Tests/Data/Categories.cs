namespace Tripsters.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Tripsters.Data.Models;

    public static class Categories
    {
        public static IEnumerable<Category> ThreeCategories
           => Enumerable.Range(0, 3).Select(i => new Category
           {
               Name = "name",
               Id=1,
           });
    }
}
