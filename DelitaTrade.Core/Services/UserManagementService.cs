using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.UsersManagementModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static DelitaTrade.Common.Constants.DelitaIdentityConstants.RoleNames;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class UserManagementService(IRepository repo, UserManager<DelitaUser> userManager) : BaseService, IUserManagementService
    {
        public async Task<IEnumerable<UserManagementViewModel>> GetAllUsersAsync(UserViewModel adminUser)
        {
            if(IsAtLeastInOneRole(adminUser, AdminRole) == false)
            {
                throw new UnauthorizedAccessException("You do not have permission to view users.");
            }

            var users = await repo.AllReadonly<DelitaUser>()
                .Where(u => u.Id != adminUser.Id)
                .ToArrayAsync();

            List<UserManagementViewModel> userManagementModels = new List<UserManagementViewModel>();

            foreach (var user in  users)
            {
                userManagementModels.Add(new UserManagementViewModel
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.Name,
                    LastName = user.LastName,
                    Roles = await userManager.GetRolesAsync(user)
                });
            }

            return userManagementModels;
        }

        public async Task<bool> AssignRoleAsync(UserViewModel adminUser, RoleAssignInputModel inputModel)
        {
            if (inputModel == null || inputModel.UserId == Guid.Empty || string.IsNullOrEmpty(inputModel.Role))
            {
                return false;
            }
            if (IsAtLeastInOneRole(adminUser, AdminRole) == false)
            {
                throw new UnauthorizedAccessException("You do not have permission to view users.");
            }

            var user = await userManager.FindByIdAsync(inputModel.UserId.ToString()) ?? throw new ArgumentNullException(NotFound(nameof(DelitaUser)));

            var result = await userManager.AddToRoleAsync(user, inputModel.Role);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));               
            }

        }

        public async Task<bool> RemoveRoleAsync(UserViewModel adminUser, RoleAssignInputModel inputModel)
        {
            if (inputModel == null || inputModel.UserId == Guid.Empty || string.IsNullOrEmpty(inputModel.Role))
            {
                return false;
            }
            if (IsAtLeastInOneRole(adminUser, AdminRole) == false)
            {
                throw new UnauthorizedAccessException("You do not have permission to view users.");
            }

            var user = await userManager.FindByIdAsync(inputModel.UserId.ToString()) ?? throw new ArgumentNullException(NotFound(nameof(DelitaUser)));

            var result = await userManager.RemoveFromRoleAsync(user, inputModel.Role);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
