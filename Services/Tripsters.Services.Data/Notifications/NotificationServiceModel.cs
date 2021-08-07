namespace Tripsters.Services.Data.Notifications
{
    public class NotificationServiceModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string UserId { get; set; }

        public string FriendId { get; set; }

        public bool IsSeen { get; set; }
    }
}
