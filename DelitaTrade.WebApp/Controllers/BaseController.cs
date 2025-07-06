using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DelitaTrade.WebApp.Controllers
{
    [Authorize]    
    public abstract class BaseController(UserManager<DelitaUser> userManager) : Controller
    {
        protected bool IsUserAuthenticated()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }

        protected Guid GetUserId()
        {
            Guid userId = Guid.Empty;

            if (IsUserAuthenticated())
            {
                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim, out userId))
                {
                    return userId;
                }
            }

            return userId;
        }

        protected string GetUserName()
        {
            return User.Identity?.Name ?? string.Empty;
        }

        protected async Task<string> GetUserFullName()
        {
            string fullName = string.Empty;
            if (IsUserAuthenticated())
            {
                var user = await userManager.GetUserAsync(User);
                fullName = user != null
                    ? $"{user.Name} {user.LastName}"
                    : string.Empty;
            }
            return fullName;
        }

        protected async Task<UserViewModel> GetUserViewModelAsync()
        {
            var userId = GetUserId();
            var userName = GetUserName();
            var roles = User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToArray();
            var fullName = await GetUserFullName();
            return new UserViewModel
            {
                Id = userId,
                UserName = userName,
                Name = fullName,
                Roles = roles
            };
        }

        protected bool IsUserInRole(params string[] roles)
        {
            return IsUserAuthenticated() && roles.Any(User.IsInRole);
        }

    }
}
