namespace Tripsters.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using Tripsters.Common;
    using Tripsters.Data.Models;

    internal class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoleAsync(dbContext, roleManager, userManager, GlobalConstants.AdministratorRoleName);
        }

        private static async Task SeedRoleAsync(ApplicationDbContext dbContext, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var newRole = new ApplicationRole(roleName);
                var result = await roleManager.CreateAsync(newRole);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                var user = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    UserName = "admin",
                };

                dbContext.Users.Add(user);

                var adminPassword = "admin123";

                await userManager.CreateAsync(user, adminPassword);

                var userRoles = new IdentityUserRole<string>
                {
                    UserId = user.Id,
                    RoleId = newRole.Id,
                };

                dbContext.UserRoles.Add(userRoles);
            }
        }
    }
}
