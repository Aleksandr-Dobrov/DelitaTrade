using DelitaTrade.Commands;
using DelitaTrade.Common.Constants;
using DelitaTrade.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using DelitaTrade.ViewModels.Controllers;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Commands.AddNewCompanyCommands;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class InitialInformationViewModel : ViewModelBase
    {
        private DateTime _date = DateTime.Now;
        private CompaniesDataManager _companiesDataManager;
        private SearchBoxTextNotUpperViewModel _returnProtocolPayMethod;
        private ReturnProtocolViewModel _selectedReturnProtocol;
        private ReturnProtocolViewModel _currentReturnProtocol;
        private TradersListViewModel _tradersViewModel;
        private ReturnProtocolExportCommandsViewModel _returnProtocolExportCommandsViewModel;
        private ImportProductsController _importProductsController;
        private readonly DateIntervalViewModel _dateIntervalViewModel;

        private ObservableCollection<ReturnProtocolViewModel> _returnProtocols = new();
        private IServiceProvider _serviceProvider;
        private Task _loadProductsTask;

        private string _protocolFilter = string.Empty;
        private bool _isDropDowOpen;
        private bool _returnProtocolOnSelected;
        private bool _objectIsLoading;
        private bool _isCompanyObjectLoadFromProtocol;

        public InitialInformationViewModel(TradersListViewModel tradersViewModel, CompaniesDataManager companiesDataManager, IServiceProvider serviceProvider, ReturnProtocolExportCommandsViewModel returnProtocolExportCommandsViewModel, ImportProductsController importProductsController, DateIntervalViewModel dateIntervalViewModel)
        {
            _tradersViewModel = tradersViewModel;
            _companiesDataManager = companiesDataManager;
            _serviceProvider = serviceProvider;
            _returnProtocolExportCommandsViewModel = returnProtocolExportCommandsViewModel;
            _importProductsController = importProductsController;
            _dateIntervalViewModel = dateIntervalViewModel;
            _returnProtocolPayMethod = new SearchBoxTextNotUpperViewModel(CreatePayMethods(), "Pay Method");
            CompaniesDataManager.CompanyObjects.ValueSelected += OnCompanyObjectSelected;
            CreateReturnProtocolCommand = new NotAsyncDefaultCommand(CreateReturnProtocol, CanCreateReturnProtocol,
                                                                     [ReturnProtocolPayMethod, TradersViewModel.TraderViewModel, CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox],
                                                                     nameof(ReturnProtocolPayMethod.Item),
                                                                     nameof(TradersViewModel.TraderViewModel.TextValue),
                                                                     nameof(CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.Value.Value));
            UpdateReturnProtocolCommand = new NotAsyncDefaultCommand(OnReturnProtocolChange, CanUpdateReturnProtocol,
                                                                     [this, ReturnProtocolPayMethod, TradersViewModel.TraderViewModel, CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox],
                                                                     nameof(Date),
                                                                     nameof(ReturnProtocolPayMethod.Item),
                                                                     nameof(TradersViewModel.TraderViewModel.TextValue),
                                                                     nameof(CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.Value.Value),
                                                                     nameof(SelectedReturnProtocol));
            DeleteReturnProtocolCommand = new NotAsyncDefaultCommand(DeleteReturnProtocol, CanDeleteReturnProtocol, this, nameof(SelectedReturnProtocol));
            _companiesDataManager.RemoveCompanyReferences();
            PropertyChanged += OnCurrentViewModelPropertyChange;
            CompaniesDataManager.CompanyObjects.ValueSelected += SetTraderBySelectedObject;
            CreateReturnProtocolEvent += AddProtocol;
            CreateReturnProtocolEvent += SetCurrentProtocol;
            SelectedReturnProtocolEvent += ReturnProtocolExportCommandsViewModel.OnReturnProtocolSelected;
            CreateReturnProtocolEvent += ReturnProtocolExportCommandsViewModel.OnReturnProtocolSelected;
            DeleteReturnProtocolEvent += ReturnProtocolExportCommandsViewModel.OnDayReportUnselected;
            DateIntervalViewModel.DateIntervalChanged += OnDateIntervalChange;
        }

        public CompaniesDataManager CompaniesDataManager => _companiesDataManager;
        public TradersListViewModel TradersViewModel => _tradersViewModel;
        public SearchBoxTextNotUpperViewModel ReturnProtocolPayMethod => _returnProtocolPayMethod;
        public ObservableCollection<ReturnProtocolViewModel> ReturnProtocols => _returnProtocols;
        public Task LoadProductsTask => _loadProductsTask;
        public ReturnProtocolExportCommandsViewModel ReturnProtocolExportCommandsViewModel => _returnProtocolExportCommandsViewModel;
        public ImportProductsController ImportProductsController => _importProductsController;
        public DateIntervalViewModel DateIntervalViewModel => _dateIntervalViewModel;

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChange();
            }
        }
        public ReturnProtocolViewModel SelectedReturnProtocol
        {
            get => _selectedReturnProtocol;
            set
            {
                _selectedReturnProtocol = value;
                if (value != null)
                {
                    _returnProtocolOnSelected = true;
                    _objectIsLoading = true;
                    _isCompanyObjectLoadFromProtocol = true;

                    Date = value.ReturnedDate;
                    CompaniesDataManager.Companies.CompaniesSearchBox.SetSelectedValue(value.CompanyObject.Company);
                    CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SetSelectedValue(value.CompanyObject);
                    TradersViewModel.TraderViewModel.TextValue = value.Trader.Name;
                    ReturnProtocolPayMethod.Item = value.PayMethod;
                    _returnProtocolOnSelected = false;
                }
                if (value != null && value != _currentReturnProtocol)
                {
                    _loadProductsTask = LoadReturnedProducts(_selectedReturnProtocol);
                    LoadProductsCallBack();
                }
                OnPropertyChange(nameof(SelectedReturnProtocol));
                OnPropertyChange(nameof(IsReturnProtocolSelected));
            }
        }

        public string ProtocolFilter
        {
            get => _protocolFilter;
            set
            {
                _protocolFilter = value;
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

        public bool IsReturnProtocolSelected => SelectedReturnProtocol != null;

        public ICommand CreateReturnProtocolCommand { get; }
        public ICommand UpdateReturnProtocolCommand { get; }
        public ICommand DeleteReturnProtocolCommand { get; }


        public event Action<ReturnProtocolViewModel> CreateReturnProtocolEvent;
        public event Action<ReturnProtocolViewModel> UpdateReturnProtocolEvent;
        public event Action<ReturnProtocolViewModel> SelectedReturnProtocolEvent;
        public event Action<ReturnProtocolViewModel> DeleteReturnProtocolEvent;

        public void CreateReturnProtocol()
        {
            using var scope = _serviceProvider.CreateScope();
            var userService = scope.GetService<UserController>();
            CreateReturnProtocolEvent(new ReturnProtocolViewModel
            {
                ReturnedDate = Date,
                PayMethod = ReturnProtocolPayMethod.Item,
                CompanyObject = new CompanyObjectViewModel
                {
                    Id = CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SelectedValue!.Id,
                    Name = CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SelectedValue.Name,
                    Address = CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SelectedValue.Address,
                    Company = new CompanyViewModel
                    {
                        Id = CompaniesDataManager.Companies.CompaniesSearchBox.SelectedValue!.Id,
                        Name = CompaniesDataManager.Companies.CompaniesSearchBox.SelectedValue.Name
                    }
                },
                Trader = new TraderViewModel
                {
                    Id = TradersViewModel.Trader.Id,
                    Name = TradersViewModel.Trader.Name
                },
                User = new UserViewModel
                {
                    Id = userService.Id,
                    Name = userService.Name,
                },
                Products = []

            });
        }

        public void OnLogOut()
        {
            SelectedReturnProtocol = null!;
            _currentReturnProtocol = null!;
            _returnProtocols.Clear();
            _returnProtocolExportCommandsViewModel.OnDayReportUnselected(null);
        }

        private ObservableCollection<string> CreatePayMethods()
        {
            return
            [
                ReturnProtocolPayMethods.BankPay,
                ReturnProtocolPayMethods.Deducted,
                ReturnProtocolPayMethods.NotDeducted,
                ReturnProtocolPayMethods.ForCancellation
            ];
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
            var userController = scope.GetService<UserController>();
            var protocols = await service.GetFilteredAsync(userController.CurrentUser!, ProtocolFilter.Split(" ", StringSplitOptions.RemoveEmptyEntries), DateIntervalViewModel.StartDate, DateIntervalViewModel.EndDate);

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

        private void SetTraderBySelectedObject(CompanyObjectViewModel companyObjectViewModel)
        {
            if (_objectIsLoading)
            {
                _objectIsLoading = false;
                return;
            }

            TradersViewModel.TraderViewModel.TextValue = companyObjectViewModel.Trader?.Name ?? null!;
        }

        private void OnCompanyObjectSelected(CompanyObjectViewModel companyObject)
        {
            if (companyObject == null) return;

            if (_isCompanyObjectLoadFromProtocol)
            {
                _isCompanyObjectLoadFromProtocol = false;
                return;
            }

            if (companyObject.IsBankPay) ReturnProtocolPayMethod.Item = ReturnProtocolPayMethods.BankPay;
            else ReturnProtocolPayMethod.Item = ReturnProtocolPayMethods.NotDeducted;
        }

        private bool CanCreateReturnProtocol()
        {
            if (ReturnProtocolPayMethod.Item == null) return false;
            if (CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SelectedValue == null) return false;
            if (TradersViewModel.TraderViewModel.Value.Value == null || TradersViewModel.Trader.Name != TradersViewModel.TraderViewModel.TextValue) return false;

            return true;
        }

        private bool CanUpdateReturnProtocol()
        {
            if (SelectedReturnProtocol == null || _returnProtocolOnSelected) return false;
            if (CanCreateReturnProtocol() == false) return false;

            if (SelectedReturnProtocol.ReturnedDate != Date) return true;
            if (SelectedReturnProtocol.PayMethod != ReturnProtocolPayMethod.Item) return true;
            if (SelectedReturnProtocol.CompanyObject.Id != CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SelectedValue?.Id) return true;
            if (TradersViewModel.Trader != null && SelectedReturnProtocol.Trader.Id != TradersViewModel.Trader.Id) return true;

            return false;
        }

        private bool CanDeleteReturnProtocol()
        {
            if (SelectedReturnProtocol == null) return false;
            return true;
        }

        private void OnReturnProtocolChange()
        {
            UpdateReturnProtocol();
            UpdateReturnProtocolEvent(SelectedReturnProtocol);
            OnPropertyChange(nameof(SelectedReturnProtocol));
        }

        private async void LoadProductsCallBack()
        {
            await LoadProductsTask;
            SelectedReturnProtocolEvent(_selectedReturnProtocol);
        }

        private void OnDateIntervalChange()
        {
            UpdateReturnProtocolsAsync();
            IsProtocolsDropDownOpen = true;
        }

        private void UpdateReturnProtocol()
        {
            if (SelectedReturnProtocol == null) throw new ArgumentNullException("Return protocol not selected");
            SelectedReturnProtocol.PayMethod = ReturnProtocolPayMethod.Item;
            SelectedReturnProtocol.ReturnedDate = Date;
            SelectedReturnProtocol.CompanyObject = CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.SelectedValue!;
            SelectedReturnProtocol.Trader = TradersViewModel.Trader;
        }
    }
}
