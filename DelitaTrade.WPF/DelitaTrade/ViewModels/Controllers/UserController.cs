using DelitaTrade.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.ViewModels.Controllers
{
    public class UserController
    {
        private UserViewModel? _userViewModel;
        public Guid Id => _userViewModel != null ? _userViewModel.Id : Guid.NewGuid();
        public string Name => _userViewModel != null ? _userViewModel.Name : "No LogIn";
        public DateTime LogInTime { get; private set; }

        public UserViewModel? CurrentUser => _userViewModel;

        public event Action<UserViewModel>? UserLogIn;
        public event Action? UserLogout;

        public void LogIn(UserViewModel user)
        {
            _userViewModel = new UserViewModel
            {
                Id = user.Id,
                Name = user.Name,                
            };

            LogInTime = DateTime.Now;
            UserLogIn?.Invoke(_userViewModel);
        }

        public void LogOut()
        {
            _userViewModel = null;
            LogInTime = DateTime.MinValue;
            UserLogout?.Invoke();
        }
    }
}
