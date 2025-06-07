using DelitaTrade.Common;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Components.ComponentsViewModel.LoginViewModelComponents;
using DelitaTrade.WPFCore.Contracts;
using System.Windows;

namespace DelitaTrade.ViewModels.Controllers
{
    public class UserLoginController
    {
        private bool _isOnCreateAccount;
        private readonly UserController _userController;
        private readonly IUserService _userService;
        private readonly LoginRememberController _rememberController;

        public UserLoginController(UserController userController, IUserService userService, LoginRememberController rememberController)
        {
            _userController = userController;
            _userService = userService;
            _rememberController = rememberController;
        }

        public event Action? OnLoginSuccess;
        public event Action? OnLogout;
        public event Action? OnInitiateCreateAccount;
        public event Action? OnAccountCreated;

        public string LoginButtonText => IsLogged ? "Log out" : "Log in";

        public string SingUpButtonText => _isOnCreateAccount == false ? "Create" : "Sing up";

        public bool IsLogged => _userController.CurrentUser != null;

        public bool IsOnCreateAccount => _isOnCreateAccount;

        public async Task Login(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, bool isRememberMe = false)
        {
            var user = new UserValidationForm { LoginName = userName.TextValue, Password = password.Password };
            await LogIn(user, userName, password, isRememberMe);
        }

        public void LogOut(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password)
        {
            userName.IsEditable = true;
            password.Visibility = Visibility.Visible;
            _userController.LogOut();
            _rememberController.ClearRememberUser();
            OnLogout?.Invoke();
        }

        public async Task InitiateCreateAccount(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, PasswordBoxViewModel confirmPassword, bool isRememberMe = false)
        {
            if (_isOnCreateAccount == false)
            {
                userName.IsEditable = true;
                password.Visibility = Visibility.Visible;                
                confirmPassword.Visibility = Visibility.Visible;
                password.Validate(password, nameof(ValidatedPasswordBox.Password));
                _isOnCreateAccount = true;
                OnInitiateCreateAccount?.Invoke();
            }
            else
            {
                userName.Validate(userName, nameof(UserNamePlaceHolderViewModel.TextValue));

                if (password.HasErrors == false && userName.HasErrors == false)
                {
                    var newUser = new UserValidationForm { LoginName = userName.TextValue, Password = password.Password };

                    if (await _userService.IsUserExist(newUser) == false && confirmPassword.HasErrors == false)
                    {
                        await _userService.CreateUser(newUser);                        
                    }

                    await LogIn(newUser, userName, password, isRememberMe);
                    SingUpSuccess(confirmPassword);
                }
            }
        }

        public UserValidationForm? TryLoadRememberUser()
        {
            return _rememberController.TryLoadRememberUser();            
        }

        private void LoginSuccess(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, bool isRememberMe)
        {
            if (isRememberMe)
            {
                _rememberController.SaveRememberUser(userName.TextValue, password.Password);
            }
            else
            {
                _rememberController.ClearRememberUser();
            }
                userName.IsEditable = false;
            password.Visibility = Visibility.Collapsed;
            password.Password = string.Empty;
            OnLoginSuccess?.Invoke();
        }

        private void SingUpSuccess(PasswordBoxViewModel confirmPassword)
        {
            confirmPassword.Visibility = Visibility.Collapsed;
            confirmPassword.Password = string.Empty;
            _isOnCreateAccount = false;            
            OnAccountCreated?.Invoke();
        }

        private async Task LogIn(UserValidationForm user, UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, bool isRememberMe)
        {
            var loggedUser = await _userService.LogIn(user);
            _userController.LogIn(loggedUser);
            if (IsLogged)
            {
                LoginSuccess(userName, password, isRememberMe);
            }
        }
    }
}
