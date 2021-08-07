namespace Tripsters.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsSeen { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string FriendId { get; set; }
    }
}
