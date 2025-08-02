using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.UsersManagementModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IUserManagementService
    {
        Task<IEnumerable<UserManagementViewModel>> GetAllUsersAsync(UserViewModel user);
        Task<bool> AssignRoleAsync(UserViewModel adminUser, RoleAssignInputModel inputModel);
        Task<bool> RemoveRoleAsync(UserViewModel adminUser, RoleAssignInputModel inputModel);
    }
}
