namespace Tripsters.Common
{
    public static class GlobalConstants
    {
        public const string SystemName = "Tripsters";

        public const string AdministratorRoleName = "Administrator";

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
