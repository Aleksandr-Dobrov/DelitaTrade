using DelitaTrade.Common;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.WPFCore.Contracts
{
    public interface IUserService
    {
        Task<UserViewModel> LogIn(UserValidationForm form);

        Task CreateUser(UserValidationForm form);

        Task<bool> IsUserExist(UserValidationForm form);
    }
}
