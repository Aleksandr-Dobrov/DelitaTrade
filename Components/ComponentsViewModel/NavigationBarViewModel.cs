using DelitaTrade.Areas.DayReportAreas;
using DelitaTrade.Commands;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private const string _logoFilePath = "\\Components\\ComponentAssets\\delitaTradeLogo.png";
        private const string _optionsImage = "\\Components\\ComponentAssets\\NavigationBar\\settings.png";
        private const string _payDeskImage = "\\Components\\ComponentAssets\\NavigationBar\\money.png";
        private const string _companiesImage = "\\Components\\ComponentAssets\\NavigationBar\\data.png";
        private const string _dayReportImage = "\\Components\\ComponentAssets\\NavigationBar\\daily-report.png";
        private const string _returnProtocolImage = "\\Components\\ComponentAssets\\NavigationBar\\pngegg.png";
        private const string _loginImage = "\\Components\\ComponentAssets\\NavigationBar\\user.png";

        private string _logoFullFilePath;
        private bool _isEditable;

        public NavigationBarViewModel(ViewModelsStore viewModelsStore, UserController userController)
        {   
            NavigateCompanyDataBaseCommand = new NavigationCommand<AddNewCompanyViewModel>
                (new NavigationService<AddNewCompanyViewModel>(viewModelsStore));
            NavigateDayReportCommand = new NavigationCommand<DayReportArea>
                (new NavigationService<DayReportArea>(viewModelsStore));
            NavigatePayDeskCommand = new NavigationCommand<PayDeskViewModel>
                (new NavigationService<PayDeskViewModel>(viewModelsStore));
            NavigateReturnProtocolCommand = new NavigationCommand<ReturnProtocolController>
                (new NavigationService<ReturnProtocolController>(viewModelsStore));
            NavigateOptionsCommand = new NavigationCommand<OptionsViewModel>
                (new NavigationService<OptionsViewModel>(viewModelsStore));
            NavigateLoginCommand = new NavigationCommand<LoginViewModel>
                (new NavigationService<LoginViewModel>(viewModelsStore));
            LogoFullFilePath = _logoFilePath;
            userController.UserLogIn += OnLogin;
            userController.UserLogout += OnLogout;
        }

        public string LogoFullFilePath
        {
            get => _logoFullFilePath;
            set
            {
                _logoFullFilePath = value.GetFullFilePathExt();
                OnPropertyChange(nameof(LogoFullFilePath));
            }
        }

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChange();
            }
        }

        public string OptionsImage => _optionsImage.GetFullFilePathExt();
        public string PayDeskImage => _payDeskImage.GetFullFilePathExt();
        public string CompaniesImage => _companiesImage.GetFullFilePathExt();
        public string DayReportImage => _dayReportImage.GetFullFilePathExt();
        public string ReturnProtocolImage => _returnProtocolImage.GetFullFilePathExt();
        public string LoginImage => _loginImage.GetFullFilePathExt();

        public ICommand NavigateCompanyDataBaseCommand { get; }

        public ICommand NavigateDayReportCommand { get; }

        public ICommand NavigatePayDeskCommand { get; }

        public ICommand NavigateReturnProtocolCommand {  get; }

        public ICommand NavigateOptionsCommand { get; }

        public ICommand NavigateLoginCommand { get; }

        private void OnLogin(UserViewModel userViewModel)
        {
            IsEditable = true;
        }

        private void OnLogout()
        {
            IsEditable = false;
        }
    }
}
