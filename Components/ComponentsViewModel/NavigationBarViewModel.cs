using DelitaTrade.Commands;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.ReturnProtocolViewModels;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class NavigationBarViewModel : ViewModelBase
    {
        private const string _logoFilePath = "\\Components\\ComponentAssets\\delitaTradeLogo.png";
        private string _logoFullFilePath;

        public NavigationBarViewModel(ViewModelsStore viewModelsStore)
        {   
            NavigateCompanyDataBaseCommand = new NavigationCommand<AddNewCompanyViewModel>
                (new NavigationService<AddNewCompanyViewModel>(viewModelsStore));
            NavigateDayReportCommand = new NavigationCommand<DayReportsViewModel>
                (new NavigationService<DayReportsViewModel>(viewModelsStore));
            NavigatePayDeskCommand = new NavigationCommand<PayDeskViewModel>
                (new NavigationService<PayDeskViewModel>(viewModelsStore));
            NavigateReturnProtocolCommand = new NavigationCommand<ReturnProtocolViewModel>
                (new NavigationService<ReturnProtocolViewModel>(viewModelsStore));
            NavigateOptionsCommand = new NavigationCommand<OptionsViewModel>
                (new NavigationService<OptionsViewModel>(viewModelsStore));
            LogoFullFilePath = _logoFilePath;
        }

        public string LogoFullFilePath
        {
            get => _logoFullFilePath;
            set
            {
                _logoFullFilePath = FileSoursePath.GetFullFilePath(value);
                OnPropertyChange(nameof(LogoFullFilePath));
            }
        }

        public ICommand NavigateCompanyDataBaseCommand { get; }

        public ICommand NavigateDayReportCommand { get; }

        public ICommand NavigatePayDeskCommand { get; }

        public ICommand NavigateReturnProtocolCommand {  get; }

        public ICommand NavigateOptionsCommand { get; }
    }
}
