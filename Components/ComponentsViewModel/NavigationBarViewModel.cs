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
        private const string _optionsImage = "\\Components\\ComponentAssets\\NavigationBar\\settings.png";
        private const string _payDeskImage = "\\Components\\ComponentAssets\\NavigationBar\\money.png";
        private const string _companiesImage = "\\Components\\ComponentAssets\\NavigationBar\\data.png";
        private const string _dayReportImage = "\\Components\\ComponentAssets\\NavigationBar\\daily-report.png";
        private const string _returnProtocolImage = "\\Components\\ComponentAssets\\NavigationBar\\pngegg.png";

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

        public string OptionsImage => FileSoursePath.GetFullFilePath(_optionsImage);
        public string PayDeskImage => FileSoursePath.GetFullFilePath (_payDeskImage);
        public string CompaniesImage => FileSoursePath.GetFullFilePath(_companiesImage);
        public string DayReportImage => FileSoursePath.GetFullFilePath (_dayReportImage);
        public string ReturnProtocolImage => FileSoursePath.GetFullFilePath (_returnProtocolImage);

        public ICommand NavigateCompanyDataBaseCommand { get; }

        public ICommand NavigateDayReportCommand { get; }

        public ICommand NavigatePayDeskCommand { get; }

        public ICommand NavigateReturnProtocolCommand {  get; }

        public ICommand NavigateOptionsCommand { get; }
    }
}
