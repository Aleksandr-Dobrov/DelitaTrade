using DelitaTrade.Common.Extensions;
using DelitaTrade.Infrastructure.Data;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Infrastructure.Seeding.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.Infrastructure.Seeding
{
    public class InitialIdentitySeeder(RoleManager<IdentityRole<Guid>> roleManager,
            UserManager<DelitaUser> userManager,
            IUserStore<DelitaUser> userStore,
            IConfiguration configurationManager) : IIdentitySeeder
    {
        private readonly Dictionary<string, string> _roleUser = new()
        {
            { Admin, "AdminUser" },
            { LogisticsManager, "LogisticUser" },
            { Driver, "DriverUser" }
        };
            
        public async Task SeedApplicationRolesAsync()
        {
            var roles = ApplicationRoleExtensions.GetAllApplicationRoles();
            foreach (var role in roles)
            {
                if (role != null && await roleManager.RoleExistsAsync(role) == false)
                {
                    var result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                    if (result.Succeeded == false)
                    {
                        throw new InvalidOperationException($"Failed to create role '{role}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        public async Task SeedApplicationUsersAsync()
        {
            foreach (var roleUser in _roleUser)
            {
                var userSection = roleUser.Value;
                var userConfig = GetUserConfiguration(userSection);
                               
                if (IsUserExist(userConfig["UserName"]) == false)
                {
                    var newUser = await CreateUserAsync(userConfig);
                    await AssignUserToRoleAsync(newUser, roleUser.Key);
                }
            }
        }

        private async Task<DelitaUser> CreateUserAsync(Dictionary<string, string> userConfig)
        {

            var user = new DelitaUser() 
            {
                Name = userConfig["FirstName"],
                LastName = userConfig["LastName"],
            };            

            await userStore.SetUserNameAsync(user, userConfig["UserName"], CancellationToken.None);            
            await GetEmailStore().SetEmailAsync(user, userConfig["UserName"], CancellationToken.None);

            var result = await userManager.CreateAsync(user, userConfig["Password"]);

            if (result.Succeeded == false)
            {
                throw new InvalidOperationException($"Failed to create user '{userConfig["UserName"]}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }

            return user;
        }

        private async Task AssignUserToRoleAsync(DelitaUser user, string roleName)
        {
            if (await userManager.IsInRoleAsync(user, roleName) == false)
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded == false)
                {
                    throw new InvalidOperationException($"Failed to assign user '{user.UserName}' to role '{roleName}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private Dictionary<string, string> GetUserConfiguration(string userConfigType)
        {
            var userConfig = configurationManager.GetSection($"DelitaUsers:{userConfigType}");
            if (userConfig == null)
            {
                throw new InvalidOperationException($"Configuration for '{userConfigType}' not found.");
            }
            var userData = userConfig.Get<Dictionary<string, string>>() ?? throw new InvalidOperationException($"Configuration for '{userConfigType}' is empty or invalid.");
            
            foreach (var key in userData.Keys)
            {
                if (string.IsNullOrWhiteSpace(userData[key]))
                {
                    throw new InvalidOperationException($"Configuration for '{key}' in '{userConfigType}' cannot be null or empty.");
                }
            }
            return userData;
        }

        private bool IsUserExist(string userName)
        {
            var existUser = userStore.FindByNameAsync(userName, CancellationToken.None).Result;
            return existUser != null;
        }
        private IUserEmailStore<DelitaUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<DelitaUser>)userStore;
        }
    }
}
