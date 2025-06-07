using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIIdentityOptionsExtension
    {
        public static IdentityOptions ApplicationIdentityConfiguration(this IdentityOptions options)
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;

            return options;
        }

        public static IdentityBuilder AddApplicationIdentityServices(this IdentityBuilder builder)
        {

            builder.AddUserManager<UserManager<DelitaUser>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddSignInManager<SignInManager<DelitaUser>>()
            .AddUserStore<UserStore<DelitaUser, IdentityRole<Guid>, DelitaDbContext, Guid>>()
            .AddEntityFrameworkStores<DelitaDbContext>()
            .AddDefaultTokenProviders();

            return builder;
        }
    }
}
