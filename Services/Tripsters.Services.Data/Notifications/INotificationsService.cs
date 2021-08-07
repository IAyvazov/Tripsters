namespace Tripsters.Services.Data.Notifications
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationsService
    {
        Task Notifie(string currUserId, string userId, string text);

        ICollection<NotificationServiceModel> GetAllNotification(string userId);

        Task Seen(int notificationId);
    }
}
