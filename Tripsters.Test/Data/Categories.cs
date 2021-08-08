using System.Collections.Generic;
using System.Linq;
using Tripsters.Data.Models;

namespace Tripsters.Test.Data
{
    public class Categories
    {
        public static IEnumerable<Category> ThreeCategories
           => Enumerable.Range(0, 3).Select(i => new Category
           {
               Name = "categoryName",
           });
    }
}
