namespace Tripsters.Tests.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Tripsters.Services.Data.Users.Models;

    public class Users
    {

        public static IEnumerable<UserServiceModel> UserFriends
        => Enumerable.Range(0, 4).Select(i => new UserServiceModel
        {
            UserName = "username",
            Id="userId",
        });

        public static UserProfileServiceModel User = new UserProfileServiceModel { UserId = "userId", Friends = UserFriends.ToList() };
    }
}
