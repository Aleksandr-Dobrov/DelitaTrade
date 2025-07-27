using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using DelitaTrade.Infrastructure.Seeding.Interfaces;
using DelitaTrade.Infrastructure.Seeding;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIServiceCollectionExtension
    {
        public static IServiceCollection AddWebApplicationIdentity(this IServiceCollection services) 
        {
            services.AddIdentity<DelitaUser, IdentityRole<Guid>>(options =>
            {
                options.ApplicationIdentityConfiguration();
            })
                .AddApplicationIdentityServices()
                .AddDefaultUI();
            services.AddTransient<IIdentitySeeder, InitialIdentitySeeder>();
            return services;
        }
    }
}
