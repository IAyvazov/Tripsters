namespace Tripsters.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Tripsters.Services.Data.Users;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

       
    }
}
