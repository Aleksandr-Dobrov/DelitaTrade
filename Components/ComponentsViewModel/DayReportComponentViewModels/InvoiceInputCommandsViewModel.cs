using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Extensions;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class InvoiceInputCommandsViewModel(IServiceProvider serviceProvider) : ViewModelBase
    {
        private const string _addInvoiceButtonImage = "Components\\ComponentAssets\\DayReport\\add.png";
        private const string _updateInvoiceButtonImage = "Components\\ComponentAssets\\DayReport\\update.png";
        private const string _deleteInvoiceButtonImage = "Components\\ComponentAssets\\DayReport\\remove.png";

        private const string _lightGreenColor = "#FF90EE90";
        private const string _orangeYellowColor = "#FFFFBD00";
        private string _addButtonColor = _lightGreenColor;

        private bool _isInitialized;

        private InvoiceCompaniesInputViewModel _companiesViewModel;
        private InvoiceCurrencyInputViewModel _currencyViewModel;
        private DayReportViewModel? _dayReportViewModel;
        private Core.ViewModels.InvoiceViewModel? _invoiceViewModel;

        private bool _commandsEnable = true;

        public event Action<Core.ViewModels.InvoiceViewModel>? InvoiceCreated;

        public bool IsInitialized => _isInitialized;
        public string AddInvoiceButtonImage => _addInvoiceButtonImage.GetFullFilePath();
        public string UpdateInvoiceButtonImage => _updateInvoiceButtonImage.GetFullFilePath();
        public string DeleteInvoiceButtonImage => _deleteInvoiceButtonImage.GetFullFilePath();

        public bool CommandsEnable
        {
            get => _commandsEnable;
            set 
            {
                _commandsEnable = value;
                OnPropertyChange();
            }
        }

        public string AddButtonColor
        {
            get => _addButtonColor;
            set
            {
                _addButtonColor = value;
                OnPropertyChange();
            }
        }

        public ICommand? Create { get; private set; }
        public ICommand? Update { get; private set; }
        public ICommand? Delete { get; private set; }

        public void InitializedCommands(InvoiceCompaniesInputViewModel companiesViewModel, InvoiceCurrencyInputViewModel currencyViewModel)
        {
            if (_isInitialized) return;
            _companiesViewModel = companiesViewModel;
            _currencyViewModel = currencyViewModel;
            _currencyViewModel.InvoiceNumberViewModel.OnInvoiceNumberChanged += OnInvoiceNumberChange;

            Create = new DefaultCommand(CreateAsync, CanCreate,
                [
                    _companiesViewModel.CompaniesViewModel.CompaniesSearchBox,
                    _companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox,
                    _companiesViewModel.CompanyTypeViewModel,
                    _currencyViewModel.InvoiceNumberViewModel,
                    _currencyViewModel.AmountViewModel,
                    _currencyViewModel.IncomeViewModel,
                    _currencyViewModel.LabeledStringToDecimalTextBoxViewModel,
                    this
                ],
            nameof(_companiesViewModel.CompaniesViewModel.CompaniesSearchBox.TextValue),
            nameof(_companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox.TextValue),
            nameof(_companiesViewModel.CompanyTypeViewModel.TextBox),
            nameof(_currencyViewModel.InvoiceNumberViewModel),
            nameof(_currencyViewModel.AmountViewModel.TextBox),
            nameof(_currencyViewModel.IncomeViewModel.TextBox),
            nameof(_currencyViewModel.LabeledStringToDecimalTextBoxViewModel),
            nameof(OnInvoiceNumberChange)
            );

            Update = new DefaultCommand(UpdateAsync, CanUpdate,
                [
                    _companiesViewModel.CompaniesViewModel.CompaniesSearchBox,
                    _companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox,
                    _companiesViewModel.CompanyTypeViewModel,
                    _currencyViewModel.AmountViewModel,
                    _currencyViewModel.IncomeViewModel,
                    _currencyViewModel.PayMethodViewModel,
                    _currencyViewModel.LabeledStringToDecimalTextBoxViewModel,
                    this
                ],
            nameof(_companiesViewModel.CompaniesViewModel.CompaniesSearchBox.TextValue),
            nameof(_companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox.TextValue),
            nameof(_companiesViewModel.CompanyTypeViewModel.TextBox),
            nameof(_currencyViewModel.AmountViewModel.TextBox),
            nameof(_currencyViewModel.IncomeViewModel.TextBox),
            nameof(_currencyViewModel.PayMethodViewModel.TextBox),
            nameof(_currencyViewModel.LabeledStringToDecimalTextBoxViewModel),
            nameof(_invoiceViewModel));

            Delete = new DefaultCommand(DeleteAsync, CanDelete, this, nameof(_invoiceViewModel));
            _isInitialized = true;
        }

        public void SelectDayReport(DayReportViewModel dayReportViewModel)
        {
            if (dayReportViewModel != null)
            {
                _dayReportViewModel = dayReportViewModel;
                CommandsEnable = true;
            }
        }

        public void UnSelectDayReport()
        {
            _dayReportViewModel = null;
            CommandsEnable = false;
        }

        public void SelectInvoice(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _invoiceViewModel = invoiceViewModel;
            OnPropertyChange(nameof(_invoiceViewModel));
        }

        public void UnselectInvoice()
        {
            _invoiceViewModel = null;
            OnPropertyChange(nameof(_invoiceViewModel));
        }
        private async Task CreateAsync()
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var invoiceService = scope.GetService<IInvoiceInDayReportService>();
                var userService = scope.GetService<UserController>();
                var newInvoice = new Core.ViewModels.InvoiceViewModel()
                {
                    Company = _companiesViewModel.CompaniesViewModel.CompaniesSearchBox.Value.Value,
                    CompanyObject = _companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox.Value.Value,
                    Number = _currencyViewModel.InvoiceNumberViewModel.TextBox,
                    Amount = _currencyViewModel.AmountViewModel.Money,
                    Income = _currencyViewModel.IncomeViewModel.Money,
                    PayMethod = _currencyViewModel.PayMethodViewModel.CurrentPayMethod,
                    DayReport = _dayReportViewModel
                };
                newInvoice = await invoiceService.CreateAsync(newInvoice);
                InvoiceCreated?.Invoke(newInvoice);
            }
            catch (ArgumentNullException ex)
            { 
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
            }
            
        }

        private async Task UpdateAsync() 
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IInvoiceInDayReportService>();
            await service.UpdateAsync(_invoiceViewModel!);
        }
        private async Task DeleteAsync() 
        {
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IInvoiceInDayReportService>();
            await service.DeleteAsync(_invoiceViewModel!);
            UnselectInvoice();
        }

        private bool CanCreate()
        {
            return _companiesViewModel.HasError == false && _currencyViewModel.HasError == false;
        }

        private bool CanUpdate() 
        {
            return _companiesViewModel.HasError == false && _invoiceViewModel != null;
        }

        private bool CanDelete()
        {
            return _invoiceViewModel != null; 
        }

        private async void OnInvoiceNumberChange(string number)
        {           
            using var scope = serviceProvider.CreateScope();
            var invoiceService = scope.GetService<IInvoicePaymentService>();
            if (await invoiceService.IsExists(number))
            {
                if (await invoiceService.IsPay(number))
                {
                    _currencyViewModel.InvoiceNumberViewModel.InvoiceHasPaid(number);
                }
                else
                {
                    AddButtonColor = _orangeYellowColor;
                }
            }
            else
            {
                AddButtonColor = _lightGreenColor;
            }            
            OnPropertyChange(nameof(OnInvoiceNumberChange));
        }
    }
}
