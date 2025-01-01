using DelitaTrade.Commands.ReturnProtocolCommands;
using DelitaTrade.Models.ReturnProtocol;
using DelitaReturnProtocolProvider.ViewModels;
using DelitaTrade.Models.ReturnProtocolSQL;
using DelitaTrade.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Windows.Input;
using DelitaReturnProtocolProvider.Services;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class InitialInformationViewModel : ViewModelBase
    {   
        private DateTime _date = DateTime.Now;
        private readonly AddNewCompanyViewModel _addNewCompanyViewModel;
        private SearchBoxTextNotUpperViewModel _returnProtocolPayMethod;
        private ReturnProtocolViewModel _selectedReturnProtocol;
        private ReturnProtocolViewModel _curentReturnProtocol;
        private ObservableCollection<ReturnProtocolViewModel> _returnProtocols = new();
        private IServiceProvider _serviceProvider;
        private Task _loadProductsTask;

        private string __protocolFilter = string.Empty;
        private bool _isDropDowOpen;

        public InitialInformationViewModel(ViewModelBase addNewCompanyViewModel, IServiceProvider serviceProvider)
        {
            _addNewCompanyViewModel = addNewCompanyViewModel as AddNewCompanyViewModel;
            _returnProtocolPayMethod = new SearchBoxTextNotUpperViewModel(CreatePayMethods(), "Pay Method");
            _addNewCompanyViewModel.SearchBoxObject.PropertyChanged += OnViewModelPropertyChange;
            CreateReturnProtocolCommand = new CreateReturnProtocolCommand(this);
            DeleteReturnProtocolCommand = new DeleteCommand(DeleteReturnProtocol);
            _serviceProvider = serviceProvider;
            PropertyChanged += OnCurrentViewModelPropertyChange;
            CreateReturnProtocolEvent += AddProtocol;
            CreateReturnProtocolEvent += SetCurrentProtocol;
        }

        public SearchBoxViewModel SearchBox => _addNewCompanyViewModel.SearchBox;
        public SearchBoxObjectViewModel SearchBoxObject => _addNewCompanyViewModel.SearchBoxObject;
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
                    SearchBox.InputText = value.CompanyObject.Company.Name;
                    SearchBoxObject.InputTextObject = value.CompanyObject.Name;
                    Trader.Item = value.Trader.Name;
                    ReturnProtocolPayMethod.Item = value.PayMethod;
                }
                if (value != null && value != _curentReturnProtocol)
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
            CreateReturnProtocolEvent(new ReturnProtocolViewModel
            {
                ReturnedDate = Date,
                PayMethod = ReturnProtocolPayMethod.Item,
                CompanyObject = new DelitaReturnProtocolProvider.ViewModels.CompanyObjectViewModel
                {
                    Name = SearchBoxObject.CurrentCompanyObject.ObjectName,
                    Address = SearchBoxObject.CurrentCompanyObject.Adrress,                    
                    Company = new DelitaReturnProtocolProvider.ViewModels.CompanyViewModel
                    {
                        Name = SearchBox.InputText
                    }
                },
                Trader = new TraderViewModel
                {
                    Name = Trader.Item
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
            if (e.PropertyName == nameof(_addNewCompanyViewModel.SearchBoxObject.InputTextObject))
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
            var protocols = Task.Factory.StartNew(() => {
                var returnProtocolService = _serviceProvider.GetService<ReturnProtocolService>() ?? throw new InvalidOperationException($"{nameof(ReturnProtocolService)} not available");
                var protocols = returnProtocolService.GetProtocolAsync(ProtocolFilter);
                return protocols;
            });
            _returnProtocols.Clear();
            foreach (var protocol in await protocols.Result)
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
            var productService = _serviceProvider.GetService<ReturnProductService>()
                 ?? throw new InvalidOperationException($"Service: {nameof(ReturnProtocolService)} not available");
            var products = await productService.GetAllProductsAsync(returnProtocol.Id);
            returnProtocol.Products.Clear();
            foreach (var product in products)
            {
                returnProtocol.Products.Add(product);
            }   
        }

        private void SetCurrentProtocol(ReturnProtocolViewModel returnProtocol)
        {
            _curentReturnProtocol = returnProtocol;
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
