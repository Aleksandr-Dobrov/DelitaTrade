using DelitaTrade.Commands.ReturnProtocolCommands;
using DelitaTrade.Models.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocolSQL;
using DelitaTrade.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows.Input;
using DelitaTrade.Extensions;
using DelitaTrade.ViewModels.Controllers;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class InitialInformationViewModel : ViewModelBase
    {   
        private DateTime _date = DateTime.Now;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;
        private CompaniesDataManager _companiesDataManager;
        private SearchBoxTextNotUpperViewModel _returnProtocolPayMethod;
        private ReturnProtocolViewModel _selectedReturnProtocol;
        private ReturnProtocolViewModel _currentReturnProtocol;
        private ObservableCollection<ReturnProtocolViewModel> _returnProtocols = new();
        private IServiceProvider _serviceProvider;
        private Task _loadProductsTask;

        private string __protocolFilter = string.Empty;
        private bool _isDropDowOpen;

        public InitialInformationViewModel(ViewModelBase addNewCompanyViewModel, IServiceProvider serviceProvider)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel as AddNewCompanyViewModel;
            _returnProtocolPayMethod = new SearchBoxTextNotUpperViewModel(CreatePayMethods(), "Pay Method");
            _companiesDataManager = serviceProvider.GetRequiredService<CompaniesDataManager>();
            _companiesDataManager.CompanyObjects.CompanyObjectsSearchBox.PropertyChanged += OnViewModelPropertyChange;
            CreateReturnProtocolCommand = new CreateReturnProtocolCommand(this);
            DeleteReturnProtocolCommand = new DeleteCommand(DeleteReturnProtocol);
            _serviceProvider = serviceProvider;
            PropertyChanged += OnCurrentViewModelPropertyChange;
            CreateReturnProtocolEvent += AddProtocol;
            CreateReturnProtocolEvent += SetCurrentProtocol;
        }

        public CompaniesDataManager CompaniesDataManager => _companiesDataManager;
        public SearchBoxTextNotUpperDeletableItemViewModel Trader => _addNewCompanyViewModel.Trader;
        public SearchBoxTextNotUpperViewModel ReturnProtocolPayMethod => _returnProtocolPayMethod;
        public ObservableCollection<ReturnProtocolViewModel> ReturnProtocols => _returnProtocols;
        public Task LoadProductsTask => _loadProductsTask;

        public DateTime Date
        {
            get => _date; 
            set { _date = value; }
        }
        public ReturnProtocolViewModel SelectedReturnProtocol
        {
            get => _selectedReturnProtocol;
            set 
            {
                _selectedReturnProtocol = value;
                if (value != null)
                {
                    CompaniesDataManager.Companies.CompaniesSearchBox.TextValue = value.CompanyObject.Company.Name;
                    CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.TextValue = value.CompanyObject.Name;
                    Trader.Item = value.Trader.Name;
                    ReturnProtocolPayMethod.Item = value.PayMethod;
                }
                if (value != null && value != _currentReturnProtocol)
                {                     
                    _loadProductsTask = LoadReturnedProducts(_selectedReturnProtocol);
                    SelectedReturnProtocolEvent(_selectedReturnProtocol);
                }                
                OnPropertyChange(nameof(SelectedReturnProtocol));
            }
        }

        public string ProtocolFilter 
        { 
            get => __protocolFilter;
            set
            {
                __protocolFilter = value;
                OnPropertyChange();
            }
        }

        public bool IsProtocolsDropDownOpen 
        {
            get => _isDropDowOpen;
            set 
            { 
                _isDropDowOpen = value; 
                OnPropertyChange(); 
            }
        }

        public ICommand CreateReturnProtocolCommand { get; }
        public ICommand DeleteReturnProtocolCommand { get; }

        public event Action<ReturnProtocolViewModel> CreateReturnProtocolEvent;
        public event Action<ReturnProtocolViewModel> SelectedReturnProtocolEvent;
        public event Action<ReturnProtocolViewModel> DeleteReturnProtocolEvent;

        public void CreateReturnProtocol(CreateReturnProtocolCommand command)
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.GetService<UserController>();
            if (userService.Id == -1) return;
            CreateReturnProtocolEvent(new ReturnProtocolViewModel
            {
                ReturnedDate = Date,
                PayMethod = ReturnProtocolPayMethod.Item,
                CompanyObject = new DelitaTrade.Core.ViewModels.CompanyObjectViewModel
                {
                    Name = CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.Value.Value.Name,
                    Address = CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.Value.Value.Address,
                    Company = new Core.ViewModels.CompanyViewModel
                    {
                        Name = CompaniesDataManager.Companies.CompaniesSearchBox.Value.Value.Name
                    }
                },
                Trader = new TraderViewModel
                {
                    Name = Trader.Item
                },
                User = new UserViewModel
                {
                    Id = userService.Id,
                    Name = userService.Name,
                },
                Products = []

            });
        }

        private ObservableCollection<string> CreatePayMethods()
        {
            return
            [
                ReturnProtocolPayMethods.BankPay,
                ReturnProtocolPayMethods.Deducted,
                ReturnProtocolPayMethods.NotDeducted
            ];
        }

        private void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {            
            if (e.PropertyName == nameof(_companiesDataManager.CompanyObjects.CompanyObjectsSearchBox.TextValue))
            {
                if (sender is SearchBoxObjectViewModel companyObject)
                {
                    if (companyObject.CurrentCompanyObject != null && companyObject.CurrentCompanyObject.BankPay)
                    {
                        ReturnProtocolPayMethod.Item = ReturnProtocolPayMethods.BankPay;
                    }
                    else
                    {
                        ReturnProtocolPayMethod.Item = ReturnProtocolPayMethods.NotDeducted;
                    }
                }
            }
        }

        private void OnCurrentViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ProtocolFilter))
            {
                UpdateReturnProtocolsAsync();
                IsProtocolsDropDownOpen = true;
            } 
        }

        private async void UpdateReturnProtocolsAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProtocolService>();
            var user = scope.GetService<UserController>();
            var protocols = await service.GetFilteredAsync(user.Id, ProtocolFilter);

            _returnProtocols.Clear();
            foreach (var protocol in protocols)
            {
                _returnProtocols.Add(protocol);
            }
        }

        private void AddProtocol(ReturnProtocolViewModel protocolViewModel)
        {
            _returnProtocols.Add(protocolViewModel);
        }

        private async Task LoadReturnedProducts(ReturnProtocolViewModel returnProtocol)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<IReturnProductService>();
            var products = await service.GetAllProductsAsync(returnProtocol.Id);
            returnProtocol.Products.Clear();
            foreach (var product in products)
            {
                returnProtocol.Products.Add(product);
            }   
        }

        private void SetCurrentProtocol(ReturnProtocolViewModel returnProtocol)
        {
            _currentReturnProtocol = returnProtocol;
            SelectedReturnProtocol = returnProtocol;
        }

        private void DeleteReturnProtocol()
        {
            if (SelectedReturnProtocol == null) throw new ArgumentNullException("Return protocol not selected");
            
            DeleteReturnProtocolEvent(SelectedReturnProtocol);
            ReturnProtocols.Remove(SelectedReturnProtocol);
        }
    }
}
