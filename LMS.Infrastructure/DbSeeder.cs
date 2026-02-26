using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Infrastructure
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            string[] roles = { "Admin", "User" };

            foreach (var roleName in roles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>
                    {
                        Name = roleName
                    });
                }
            }
        }
    }
}
