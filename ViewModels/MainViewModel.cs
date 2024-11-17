using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.ViewModels.ReturnProtocolViewModels;
using DelitaTrade.Models;
using DelitaTrade.Stores;
using DelitaTrade.Models.DI;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;

namespace DelitaTrade.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        IServiceProvider _serviceProvider = DIContainer.BuildServiceProvider();
        private ViewModelsStore _viewModelsStore;
        private NavigationBarViewModel _navigationBarViewModel;

        public MainViewModel(DelitaTradeCompany delitaTrade, DelitaTradeDayReport dayReportCreator)
        {
            var dayReportOptions = _serviceProvider.GetService<DayReportInputOptionsViewModelComponent>();
            ViewModelBase addCompanyViewModel = new AddNewCompanyViewModel(delitaTrade);
            ViewModelBase dayReportViewModel = new DayReportsViewModel(dayReportCreator, addCompanyViewModel, dayReportOptions);
            ViewModelBase payDeskViewModel = new PayDeskViewModel(dayReportCreator);
            ViewModelBase returnProtocolViewModel = new ReturnProtocolViewModel(addCompanyViewModel);
            ViewModelBase optionsViewModel = new OptionsViewModel(dayReportCreator, dayReportOptions);
            _viewModelsStore = new ViewModelsStore([addCompanyViewModel, dayReportViewModel, payDeskViewModel, returnProtocolViewModel, optionsViewModel]);
            _viewModelsStore.ViewModelChanged += OnViewModelChanged;
            _navigationBarViewModel = new NavigationBarViewModel(_viewModelsStore);
            _viewModelsStore.SetViewModel<AddNewCompanyViewModel>();
        }

        public ViewModelBase CurrentViewModel => _viewModelsStore.CurrentViewModel;
        public NavigationBarViewModel NavigationBarViewModel => _navigationBarViewModel;

        private void OnViewModelChanged()
        { 
            OnPropertyChange(nameof(CurrentViewModel));
        }
    }
}
