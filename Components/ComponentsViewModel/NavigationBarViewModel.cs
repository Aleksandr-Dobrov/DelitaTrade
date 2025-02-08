using DelitaTrade.Areas.DayReportAreas;
using DelitaTrade.Commands;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.ViewModels;
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

        private string _logoFullFilePath;

        public NavigationBarViewModel(ViewModelsStore viewModelsStore)
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
            LogoFullFilePath = _logoFilePath;
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

        public string OptionsImage => _optionsImage.GetFullFilePathExt();
        public string PayDeskImage => _payDeskImage.GetFullFilePathExt();
        public string CompaniesImage => _companiesImage.GetFullFilePathExt();
        public string DayReportImage => _dayReportImage.GetFullFilePathExt();
        public string ReturnProtocolImage => _returnProtocolImage.GetFullFilePathExt();

        public ICommand NavigateCompanyDataBaseCommand { get; }

        public ICommand NavigateDayReportCommand { get; }

        public ICommand NavigatePayDeskCommand { get; }

        public ICommand NavigateReturnProtocolCommand {  get; }

        public ICommand NavigateOptionsCommand { get; }
    }
}
