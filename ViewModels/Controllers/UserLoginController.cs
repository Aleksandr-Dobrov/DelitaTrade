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

        public async Task Login(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, FirstNamePlaceHolderViewModel firstName, LastNamePlaceHolderViewModel lastName, bool isRememberMe = false)
        {
            var user = new UserValidationForm { LoginName = userName.TextValue, Password = password.Password };
            await LogIn(user, userName, password, firstName, lastName, isRememberMe);
        }

        public void LogOut(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, FirstNamePlaceHolderViewModel firstName, LastNamePlaceHolderViewModel lastName)
        {
            userName.IsEditable = true;
            password.Visibility = Visibility.Visible;
            firstName.IsEditable = true;
            firstName.Visibility = Visibility.Collapsed;
            firstName.TextValue = string.Empty;
            lastName.IsEditable = true;
            lastName.Visibility = Visibility.Collapsed;
            lastName.TextValue = string.Empty;
            _userController.LogOut();
            _rememberController.ClearRememberUser();
            OnLogout?.Invoke();
        }

        public async Task InitiateCreateAccount(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, PasswordBoxViewModel confirmPassword, FirstNamePlaceHolderViewModel firstName, LastNamePlaceHolderViewModel lastName,  bool isRememberMe = false)
        {
            if (_isOnCreateAccount == false)
            {
                userName.IsEditable = true;
                password.Visibility = Visibility.Visible;                
                confirmPassword.Visibility = Visibility.Visible;
                firstName.Visibility = Visibility.Visible;
                lastName.Visibility = Visibility.Visible;
                password.Validate(password, nameof(ValidatedPasswordBox.Password));
                firstName.Validate(firstName, nameof(FirstNamePlaceHolderViewModel.TextValue));
                lastName.Validate(lastName, nameof(LastNamePlaceHolderViewModel.TextValue));
                _isOnCreateAccount = true;
                OnInitiateCreateAccount?.Invoke();
            }
            else
            {
                firstName.Validate(firstName, nameof(FirstNamePlaceHolderViewModel.TextValue));
                lastName.Validate(lastName, nameof(LastNamePlaceHolderViewModel.TextValue));
                userName.Validate(userName, nameof(UserNamePlaceHolderViewModel.TextValue));

                if (password.HasErrors == false && userName.HasErrors == false && ((firstName.HasErrors == false && lastName.HasErrors == false) || await _userService.IsUserExist(new UserValidationForm { LoginName = userName.TextValue , Password = password.Password})))
                {
                    var newUser = new UserValidationForm { LoginName = userName.TextValue, Password = password.Password, FirstName = firstName.TextValue, LastName = lastName.TextValue };

                    if (await _userService.IsUserExist(newUser) == false && confirmPassword.HasErrors == false)
                    {
                        await _userService.CreateUser(newUser);
                    }

                    await LogIn(newUser, userName, password, firstName, lastName, isRememberMe);
                    SingUpSuccess(confirmPassword, firstName, lastName);
                }
                else 
                {
                    string errorMessage = $"Please correct the following errors:{Environment.NewLine}";
                    PlaceHolderTextBoxViewModel[] placeHolders = { userName, firstName, lastName };
                    PasswordBoxViewModel[] passwordBoxes = { password, confirmPassword };
                    foreach (var placeHolder in placeHolders)
                    {
                        if (placeHolder.HasErrors)
                        {
                            var errors = placeHolder.GetErrors(nameof(placeHolder.TextValue)).Cast<string>();
                            errorMessage += $"--{placeHolder.Name}: {string.Join(", ", errors)}{Environment.NewLine}";
                        }
                    }
                    foreach (var passwordBox in passwordBoxes)
                    {
                        if (passwordBox.HasErrors)
                        {
                            var errors = passwordBox.GetErrors(nameof(passwordBox.Password)).Cast<string>();
                            errorMessage += $"--{passwordBox.Name}: {string.Join(", ", errors)}{Environment.NewLine}";
                        }
                    }

                    throw new ArgumentException(errorMessage);
                }
            }
        }

        public UserValidationForm? TryLoadRememberUser()
        {
            return _rememberController.TryLoadRememberUser();            
        }

        private void LoginSuccess(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, FirstNamePlaceHolderViewModel firstName, LastNamePlaceHolderViewModel lastName, bool isRememberMe)
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
            firstName.IsEditable = false;
            firstName.Visibility = Visibility.Visible;
            lastName.IsEditable = false;
            lastName.Visibility = Visibility.Visible;
            OnLoginSuccess?.Invoke();
        }

        private void SingUpSuccess(PasswordBoxViewModel confirmPassword, FirstNamePlaceHolderViewModel firstName, LastNamePlaceHolderViewModel lastName)
        {
            confirmPassword.Visibility = Visibility.Collapsed;
            firstName.Visibility = Visibility.Visible;
            lastName.Visibility = Visibility.Visible;
            confirmPassword.Password = string.Empty;
            _isOnCreateAccount = false;            
            OnAccountCreated?.Invoke();
        }

        private async Task LogIn(UserValidationForm user, UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, FirstNamePlaceHolderViewModel firstName, LastNamePlaceHolderViewModel lastName, bool isRememberMe)
        {
            var loggedUser = await _userService.LogIn(user);
            firstName.TextValue = loggedUser.Name.Split(' ')[0];
            lastName.TextValue = loggedUser.Name.Split(' ').Length > 1 ? loggedUser.Name.Split(' ')[1] : string.Empty;
            _userController.LogIn(loggedUser);
            if (IsLogged)
            {
                LoginSuccess(userName, password, firstName, lastName, isRememberMe);
            }
        }
    }
}
