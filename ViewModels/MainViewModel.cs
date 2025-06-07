using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.ViewModels.ReturnProtocolControllers;
using DelitaTrade.Models;
using DelitaTrade.Areas.DayReportAreas;
using DelitaTrade.Stores;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using System.Windows;
using Microsoft.Extensions.Configuration;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _version = string.Empty;
        private const string _copyRight = "©";
        private string _releaseYear = string.Empty;
        
        private ViewModelsStore _viewModelsStore;
        private NavigationBarViewModel _navigationBarViewModel;

        public MainViewModel(AddNewCompanyViewModel addCompanyViewModel, DayReportArea dayReportArea, PayDeskViewModel payDeskViewModel, ReturnProtocolController returnProtocolViewModel, OptionsViewModel optionsViewModel, LoginViewModel loginViewModel, IConfiguration configuration, UserController userController)
        {
            _version = configuration.GetSection("ApplicationVersion").GetValue(typeof(string), "Version") as string ?? string.Empty;
            _releaseYear = configuration.GetSection("ReleaseDate").GetValue(typeof(string), "Year") as string ?? string.Empty;
            dayReportArea.DayReportLoaderViewModel.DayReportSelected += payDeskViewModel.OnDayReportSelected;
            dayReportArea.DayReportLoaderViewModel.DayReportUnSelect += payDeskViewModel.OnDayReportUnselected;
            dayReportArea.DayReportLoaderViewModel.DayReportTotalsViewModel.DayReportUpdated += payDeskViewModel.GetAllBanknotes;
            _viewModelsStore = new ViewModelsStore([addCompanyViewModel, dayReportArea, payDeskViewModel, returnProtocolViewModel, optionsViewModel, loginViewModel]);
            _viewModelsStore.ViewModelChanged += OnViewModelChanged;
            _navigationBarViewModel = new NavigationBarViewModel(_viewModelsStore, userController);
            _viewModelsStore.SetViewModel<LoginViewModel>();            
        }

        public ViewModelBase CurrentViewModel => _viewModelsStore.CurrentViewModel;
        public NavigationBarViewModel NavigationBarViewModel => _navigationBarViewModel;

        private void OnViewModelChanged()
        { 
            OnPropertyChange(nameof(CurrentViewModel));
        }


        public string Version => $"Version: {_version}";

        public string CopyRight => $"{_copyRight} {_releaseYear} - {DateTime.Now.Year}";
    }
}
