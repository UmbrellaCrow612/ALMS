using ALMS.API.Core.Constants;
using Microsoft.AspNetCore.Identity;

namespace ALMS.API.Data.Seeding
{
    public static class UserRolesSeed
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in UserRoles.Roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
