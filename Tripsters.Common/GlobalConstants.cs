namespace Tripsters.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Tripsters";

        public const string AdministratorRoleName = "Administrator";

        public const string GlobalMessageKey = "GlobalMessage";

        public class Notifications
        {
            public const string FriendRequestText = "sent you friend request";

            public const string CommentText = "write a comment in ";

            public const string LikeText = "like ";

            public const string JoinText = "join in ";

            public const string PhotoLikeText = "like your photo";

            public const string BadgeText = "added badge to your profile";

            public const string ApprovedTrip = "your trip is approved";
        }

        public class TripSecurity
        {
            public const int NameMinLength = 5;
            public const int NameMaxLength = 100;

            public const int AvailableSeatsMinRange = 1;
            public const int AvailableSeatsMaxRange = 6;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 300;

            public const int DestinationMinLength = 3;
            public const int DestinationMaxLength = 50;
        }
    }
}
