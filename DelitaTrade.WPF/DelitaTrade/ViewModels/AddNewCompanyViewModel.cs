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
        private const string _searchGoogleIcon = "Components\\ComponentAssets\\googleMapIcon.png";
        private CompaniesDataManager _companiesDataManager;

        public AddNewCompanyViewModel(CompaniesDataManager companiesDataManager)
        {
            ShowOnMap = new SearchCommand(this);
            _companiesDataManager = companiesDataManager;
        }
      
        public CompaniesDataManager CompaniesDataManager => _companiesDataManager;

        public string SearchOnMapButtonImage => _searchGoogleIcon.GetFullFilePathExt();
              
        public ICommand ShowOnMap { get; }

    }
}
