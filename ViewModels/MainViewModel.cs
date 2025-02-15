using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using DelitaTrade.Models;
using DelitaTrade.Areas.DayReportAreas;
using DelitaTrade.Stores;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;

namespace DelitaTrade.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        
        private ViewModelsStore _viewModelsStore;
        private NavigationBarViewModel _navigationBarViewModel;

        public MainViewModel(AddNewCompanyViewModel addCompanyViewModel, DayReportArea dayReportArea, PayDeskViewModel payDeskViewModel, ReturnProtocolController returnProtocolViewModel, OptionsViewModel optionsViewModel)
        {
            //var dayReportOptions = serviceProvider.GetService<DayReportInputOptionsViewModelComponent>();
            //ViewModelBase dayReportViewModel = new DayReportsViewModel(dayReportCreator, addCompanyViewModel, dayReportOptions, serviceProvider);
            //ViewModelBase addCompanyViewModel = serviceProvider.GetRequiredService<AddNewCompanyViewModel>();
            //DayReportArea dayReportArea = serviceProvider.GetRequiredService<DayReportArea>();
            //PayDeskViewModel payDeskViewModel = new PayDeskViewModel(serviceProvider);
            //ViewModelBase returnProtocolViewModel = serviceProvider.GetRequiredService<ReturnProtocolController>();
            //ViewModelBase optionsViewModel = serviceProvider.GetRequiredService<OptionsViewModel>();
            dayReportArea.DayReportLoaderViewModel.DayReportSelected += payDeskViewModel.OnDayReportSelected;
            dayReportArea.DayReportLoaderViewModel.DayReportUnSelect += payDeskViewModel.OnDayReportUnselected;
            dayReportArea.DayReportLoaderViewModel.DayReportTotalsViewModel.DayReportUpdated += payDeskViewModel.GetAllBanknotes;
            _viewModelsStore = new ViewModelsStore([addCompanyViewModel, dayReportArea, payDeskViewModel, returnProtocolViewModel, optionsViewModel]);
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
