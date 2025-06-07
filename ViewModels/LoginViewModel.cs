using DelitaTrade.Commands;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Components.ComponentsViewModel.LoginViewModelComponents;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace DelitaTrade.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private Visibility _logInButtonVisibility;
        private Visibility _singUpButtonVisibility;
        private Visibility _rememberMeVisibility;
        private string _headWord = "Log in";
        private bool _isRememberMe = false;
        private bool _isNotLoaded = false;

        private UserNamePlaceHolderViewModel _userNamePlaceHolder = new UserNamePlaceHolderViewModel();

        private UserLoginController _userLoginController;


        public LoginViewModel(UserLoginController userLoginController)
        {
            _userLoginController = userLoginController;
            _userLoginController.OnLoginSuccess += OnLoginChange;
            _userLoginController.OnLogout += OnLoginChange;
            _userLoginController.OnInitiateCreateAccount += OnAccountCreated;
            _userLoginController.OnAccountCreated += OnCreatedComplete;
            ConfirmPasswordBox.PropertyChanged += OnViewModelPropertyChange;
            StartUpLogin();
            Application.Current.Startup += OnLoadComplete;
        }

        private async void OnLoadComplete(object sender, StartupEventArgs e)
        {            
            await Task.Run(() => 
            {
                Thread.Sleep(2000); //TODO Create a better solution to wait for the TradersListViewModel to load from the database.
                if (_isNotLoaded) return;
                LogCallback();
            });
        }

        public string HeadWord
        {
            get => _headWord;
            set
            {
                _headWord = value;
                OnPropertyChange();
            }
        }


        public Visibility LogInButtonVisibility
        {
            get => _logInButtonVisibility;
            set
            {
                _logInButtonVisibility = value;
                OnPropertyChange();
            }
        }

        public Visibility SingUpButtonVisibility
        {
            get => _singUpButtonVisibility;
            set
            {
                _singUpButtonVisibility = value;
                OnPropertyChange();
            }
        }

        public Visibility RememberMeVisibility
        {
            get => _rememberMeVisibility;
            set
            {
                _rememberMeVisibility = value;
                OnPropertyChange();
            }
        }

        public bool IsRememberMe
        {
            get => _isRememberMe;
            set
            {
                _isRememberMe = value;
                OnPropertyChange();
            }
        }


        public string LoginButtonText => _userLoginController.LoginButtonText;
        public string SingUpButtonText => _userLoginController.SingUpButtonText;

        public UserNamePlaceHolderViewModel UserNamePlaceHolder => _userNamePlaceHolder;

        public PasswordBoxViewModel PasswordBox { get; private set; } = new PasswordBoxViewModel("Password");

        public PasswordBoxViewModel ConfirmPasswordBox { get; } = new PasswordBoxViewModel("Confirm Password") { Visibility = Visibility.Collapsed };

        public UserLoginController UserLoginController => _userLoginController;

        private async Task Login(UserNamePlaceHolderViewModel userName, PasswordBoxViewModel password, bool isRememberMe = false)
        {
            await _userLoginController.Login(userName, password, isRememberMe);
        }

        public ICommand LogInCommand => new NotAsyncDefaultCommand(LogCallback);

        public ICommand SingUpCommand => new NotAsyncDefaultCommand(SingUpCallback);

        private void OnLoginChange()
        {
            OnPropertyChange(nameof(LoginButtonText));
            SingUpButtonVisibility = UserLoginController.IsLogged ? Visibility.Collapsed : Visibility.Visible;
            RememberMeVisibility = UserLoginController.IsLogged ? Visibility.Collapsed : Visibility.Visible;
            HeadWord = UserLoginController.IsLogged ? "Logged in" : "Log in";
        }

        private void OnAccountCreated()
        {
            LogInButtonVisibility = Visibility.Collapsed;
            HeadWord = "Sign up";
            OnPropertyChange(nameof(SingUpButtonText));
        }

        private void OnCreatedComplete()
        {
            PasswordBox.PropertyChanged -= OnViewModelPropertyChange;
            PasswordBox = new PasswordBoxViewModel("Password");
            OnPropertyChange(nameof(PasswordBox));
            PasswordBox.Visibility = Visibility.Collapsed;
            LogInButtonVisibility = Visibility.Visible;
            ConfirmPasswordBox.Visibility = Visibility.Collapsed;
            ConfirmPasswordBox.ClearErrors(nameof(PasswordBoxViewModel.Password));
            OnPropertyChange(nameof(SingUpButtonText));
            OnLoginChange();
        }

        private async void LogCallback()
        {
            try
            {
                _isNotLoaded = true;
                UserNamePlaceHolder.Validate(UserNamePlaceHolder, nameof(UserNamePlaceHolder.TextValue));
                PasswordBox.Validate(PasswordBox, nameof(ValidatedPasswordBox.Password));
                

                if (UserLoginController.IsLogged == false)
                {
                    if (UserNamePlaceHolder.HasErrors || PasswordBox.HasErrors) return;

                    await Login(UserNamePlaceHolder, PasswordBox, IsRememberMe);
                }
                else
                {
                    UserLoginController.LogOut(UserNamePlaceHolder, PasswordBox);
                }
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Models.Loggers.Logger.LogLevel.Information);
            }
        }

        private async void SingUpCallback()
        {
            try
            {
                _isNotLoaded = true;
                if (UserLoginController.IsOnCreateAccount == false)
                {
                    PasswordBox = new ValidatedPasswordBox("Password");
                    PasswordBox.PropertyChanged += OnViewModelPropertyChange;
                    OnPropertyChange(nameof(PasswordBox));
                }
                await UserLoginController.InitiateCreateAccount(UserNamePlaceHolder, PasswordBox, ConfirmPasswordBox, IsRememberMe);
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Models.Loggers.Logger.LogLevel.Information);
            }
        }

        private void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PasswordBoxViewModel.Password) || e.PropertyName == nameof(ValidatedPasswordBox.Password))
            {
                ConfirmPasswordBox.Validate("Passwords do not match", ConfirmPasswordBox.Password == PasswordBox.Password, nameof(PasswordBoxViewModel.Password));
            }
        }

        private void StartUpLogin()
        {
            var rememberUser = UserLoginController.TryLoadRememberUser();
            if (rememberUser != null) 
            {
                UserNamePlaceHolder.TextValue = rememberUser.LoginName;
                PasswordBox.Password = rememberUser.Password;
                IsRememberMe = true;
            }
        }
    }
}
