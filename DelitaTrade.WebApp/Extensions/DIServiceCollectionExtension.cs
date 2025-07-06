using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Infrastructure.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
            
            return services;
        }
    }
}
