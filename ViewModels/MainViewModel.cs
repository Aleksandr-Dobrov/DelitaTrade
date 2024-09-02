using DelitaTrade.Components;
using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models.ReturnProtocol;
using DelitaTrade.ViewModels.ReturnProtocolViewModels;
using DelitaTrade.Models;
using DelitaTrade.Stores;

namespace DelitaTrade.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelsStore _viewModelsStore;

        private NavigationBarViewModel _navigationBarViewModel;
        public ViewModelBase CurrentViewModel => _viewModelsStore.CurrentViewModel;

        public NavigationBarViewModel NavigationBarViewModel => _navigationBarViewModel;

        public MainViewModel(DelitaTradeCompany delitaTrade, DelitaTradeDayReport dayReportCreator)
        {
            ViewModelBase addCompanyViewModel = new AddNewCompanyViewModel(delitaTrade);
            ViewModelBase dayReportViewModel = new DayReportsViewModel(dayReportCreator, addCompanyViewModel);
            ViewModelBase payDeskViewModel = new PayDeskViewModel(dayReportCreator);
            ViewModelBase returnProtocolViewModel = new ReturnProtocolViewModel(addCompanyViewModel);
            _viewModelsStore = new ViewModelsStore([addCompanyViewModel, dayReportViewModel, payDeskViewModel, returnProtocolViewModel]);
            _viewModelsStore.ViewModelChanged += OnViewModelChanged;
            _navigationBarViewModel = new NavigationBarViewModel(_viewModelsStore);
            _viewModelsStore.SetViewModel<AddNewCompanyViewModel>();
        }

        private void OnViewModelChanged()
        { 
            OnPropertyChange(nameof(CurrentViewModel));
        }
    }
}
