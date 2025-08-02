using DelitaTrade.Infrastructure.Seeding.Interfaces;
using DelitaTrade.WebApp.Middlewares;

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

        /// <summary>
        /// Adds middleware to the application's request pipeline that redirects users to the admin interface based on
        /// admin role.
        /// </summary>
        /// <remarks>This extension method integrates the <see cref="AdminRedirectionMiddleware"/> into
        /// the application's request pipeline. Use this method to enable automatic redirection to the admin interface
        /// when applicable. Ensure that the middleware is added in the correct order relative to other middleware
        /// components to achieve the desired behavior.</remarks>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance to configure the middleware.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance, enabling further configuration.</returns>
        public static IApplicationBuilder UseAdminRedirection(this IApplicationBuilder app) 
        {
            app.UseMiddleware<AdminRedirectionMiddleware>();
            return app;
        }
    }
}
