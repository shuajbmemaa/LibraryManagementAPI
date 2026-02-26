using LMS.Infrastructure;

namespace LibraryManagementAPI.Extensions
{
    public static class SeedExtensions
    {
        public static async Task UseDbSeeder(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                await DbSeeder.SeedRolesAndAdminAsync(services);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Gabim gjate seeding te database.");
            }
        }
    }
}
