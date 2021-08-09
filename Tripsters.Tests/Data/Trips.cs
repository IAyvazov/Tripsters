using System.Collections.Generic;
using System.Linq;
using Tripsters.Data.Models;

namespace Tripsters.Tests.Data
{
    public static class Trips
    {
        public static IEnumerable<Trip> TripsByCategory
           => Enumerable.Range(0, 4).Select(i => new Trip
           {
               Name = "TripName",
               CategoryId = 1,
               IsApproved=true,
           });
    }
}
