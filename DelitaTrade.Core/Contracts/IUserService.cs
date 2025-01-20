using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Common;

namespace DelitaTrade.Core.Contracts
{
    public interface IUserService
    {
        Task<UserViewModel> LogIn(UserValidationForm form);

        Task CreateUser(UserValidationForm form);
    }
}
