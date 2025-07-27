using DelitaTrade.Infrastructure.Seeding.Interfaces;

namespace DelitaTrade.WebApp.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Seeds the identity data, including application roles and users, into the database.
        /// </summary>
        /// <remarks>This method ensures that the required roles and users for the application are created
        /// and persisted in the identity database. It should be called during application startup to initialize
        /// identity data. Required configuration section "DelitaUsers" whit objects "AdminUser", "LogisticUser", "DriverUser"
        /// each of them has keys: "UserName" ,"Password" ,"FirstName" ,"LastName" whit string values.</remarks> 
        /// <exception cref="InvalidOperationException">Thrown if there is an error during the seeding process.</exception>"
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance used to configure the application.</param>
        /// <returns>The same <see cref="IApplicationBuilder"/> instance to allow for method chaining.</returns>
        public static IApplicationBuilder SeedingIdentityData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var identitySeeder = scope.ServiceProvider.GetRequiredService<IIdentitySeeder>();
                identitySeeder.SeedApplicationRolesAsync().GetAwaiter().GetResult();
                identitySeeder.SeedApplicationUsersAsync().GetAwaiter().GetResult();
            }

            return app;
        }
    }
}
