namespace Tripsters.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Tripsters.Services.Data.Notifications;
    using Tripsters.Services.Data.Trips;

    using static Tripsters.Common.GlobalConstants;

    public class TripsController : AdministrationController
    {
        private readonly ITripsService tripsService;
        private readonly INotificationsService notificationsService;

        public TripsController(
            ITripsService tripsService,
            INotificationsService notificationsService)
        {
            this.tripsService = tripsService;
            this.notificationsService = notificationsService;
        }

        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Index()
        {
            var model = this.tripsService.GetAllTripsForAdmin(1, 10);
            return this.View(model);
        }

        [Authorize(Roles = AdministratorRoleName)]
        public async Task<IActionResult> Approve(string tripId)
        {
            var userId = await this.tripsService.Approve(tripId);

            await this.notificationsService.Notifie(userId, userId, Notifications.ApprovedTrip);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
