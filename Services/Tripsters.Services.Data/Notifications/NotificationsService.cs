namespace Tripsters.Services.Data.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Tripsters.Data;
    using Tripsters.Data.Models;

    public class NotificationsService : INotificationsService
    {
        private readonly ApplicationDbContext dbContext;

        public NotificationsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ICollection<NotificationServiceModel> GetAllNotification(string userId)
        {
            var notifications = this.dbContext
            .Notifications
            .Where(n => n.UserId == userId && n.IsSeen == false)
            .Select(n => new NotificationServiceModel
            {
                Id = n.Id,
                Text = n.Text,
                UserId = n.UserId,
                FriendId = n.FriendId,
                IsSeen = n.IsSeen,
            })
            .ToList();

            return notifications;
        }

        public async Task Notifie(string currUserId, string userId, string text)
        {
            var currUser = this.dbContext.Users
                .FirstOrDefault(u => u.Id == currUserId);

            var user = this.dbContext.Users
                .FirstOrDefault(u => u.Id == userId);

            if (currUser == null || user == null)
            {
                throw new NullReferenceException("There is no such user.");
            }

            var notification = new Notification
            {
                UserId = userId,
                Text = $"{currUser.UserName} {text}.",
                FriendId = currUserId,
                IsSeen = false,
            };

            await this.dbContext.Notifications.AddAsync(notification);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task Seen(int notificationId)
        {
            var notification = this.dbContext.Notifications.FirstOrDefault(n => n.Id == notificationId);
            notification.IsSeen = true;
            await this.dbContext.SaveChangesAsync();
        }
    }
}
