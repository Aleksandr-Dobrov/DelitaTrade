using DelitaTrade.Commands;
using DelitaTrade.Services;
using DelitaTrade.Stores;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.ReturnProtocolViewModels;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class NavigationBarViewModel : ViewModelBase
    {   
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
        }

        public ICommand NavigateCompanyDataBaseCommand { get; }

        public ICommand NavigateDayReportCommand { get; }

        public ICommand NavigatePayDeskCommand { get; }

        public ICommand NavigateReturnProtocolCommand {  get; }
    }
}
