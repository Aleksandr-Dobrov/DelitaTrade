using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels.UsersManagementModels;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;

namespace DelitaTrade.WebApp.Areas.Admin.Controllers
{
    public class UsersManagementController(IUserManagementService userManagementService,
                UserManager<DelitaUser> userManager) : BaseAdminController(userManager)
    {
        public async Task<IActionResult> Index()
        {
            var user = await GetUserViewModelAsync();

            var users = await userManagementService.GetAllUsersAsync(user);

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignInputModel roleAssignModel)
        {   
            if (roleAssignModel.UserId == Guid.Empty || string.IsNullOrEmpty(roleAssignModel.Role))
            {
                return BadRequest("Invalid user ID or role.");
            }

            var user = await GetUserViewModelAsync();

            bool result = await userManagementService.AssignRoleAsync(user, roleAssignModel);

            if (result == false)
            {
                return BadRequest("Failed to assign role.");
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole (RoleAssignInputModel roleAssignModel)
        {
            if (roleAssignModel.UserId == Guid.Empty || string.IsNullOrEmpty(roleAssignModel.Role))
            {
                return BadRequest("Invalid user ID or role.");
            }

            var user = await GetUserViewModelAsync();

            bool result = await userManagementService.RemoveRoleAsync(user, roleAssignModel);

            if (result == false)
            {
                return BadRequest("Failed to remove role.");
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
