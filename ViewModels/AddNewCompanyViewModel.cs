using DelitaTrade.Commands;
using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Components.ComponentsView;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace DelitaTrade.ViewModels
{
    public class AddNewCompanyViewModel : ViewModelBase
    {  
        private SearchBoxTextNotUpperDeletableItemViewModel _trader;
        private ObservableCollection<string> _tradersViewModel;
        private StringListDataBase _tradersModel;
        private CompaniesDataManager _companiesDataManager;

        public AddNewCompanyViewModel(CompaniesDataManager companiesDataManager)
        {
            _tradersViewModel = new ObservableCollection<string>();
            _tradersModel = new StringListDataBase("../../../SafeDataBase/Traders/traders.txt", "Trader");
            _trader = new SearchBoxTextNotUpperDeletableItemViewModel(_tradersViewModel, "Trader", _tradersModel);
            ShowOnMap = new SearchCommand(this);
            _companiesDataManager = companiesDataManager;
            OnEnable();
        }
      
        public SearchBoxTextNotUpperDeletableItemViewModel Trader => _trader;

        public CompaniesDataManager CompaniesDataManager => _companiesDataManager;

        public string SearchOnMapButtonImage => FileSoursePath.GetFullFilePath("Components\\ComponentAssets\\googleMapIcon.png");
              
        public ICommand ShowOnMap {  get; }

        private void OnEnable()
        {
            _tradersModel.ColectionChainge += TradersDataUpdate;
            TradersDataUpdate();
            _trader.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBoxTextNotUpperDeletableItemViewModel.Item))
            {
                string item = (sender as SearchBoxTextNotUpperDeletableItemViewModel).Item;
                if (item != null && item != string.Empty && _tradersModel.Contains(item) == false)
                {
                    _tradersModel.Add(item);
                }
            }
        }

        private void TradersDataUpdate()
        {
            _tradersViewModel.Clear();
            foreach (var trader in _tradersModel)
            {
                _tradersViewModel.Add(trader);
            }
        }
    }
}
