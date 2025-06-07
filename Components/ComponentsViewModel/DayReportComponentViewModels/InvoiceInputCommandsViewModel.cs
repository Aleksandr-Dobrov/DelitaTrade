using DelitaTrade.Commands;
using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using DelitaTrade.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Media.Media3D;

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
        private bool _isInvoiceNotPay;


        private InvoiceCompaniesInputViewModel _companiesViewModel;
        private InvoiceCurrencyInputViewModel _currencyViewModel;
        private DayReportViewModel? _dayReportViewModel;
        private InvoiceViewModel? _invoiceViewModel;

        private bool _commandsEnable = true;

        public event Action<InvoiceViewModel>? InvoiceCreated;
        public event Action<InvoiceViewModel>? InvoiceDeleted;
        public event Action<InvoiceViewModel>? InvoiceUpdated;
        public event Func<string, Task>? NotPaidInvoiceNumberSelected;
        public event Action<InvoiceViewModel>? NonPaidInvoiceLoaded;

        public bool IsInitialized => _isInitialized;
        public string AddInvoiceButtonImage => _addInvoiceButtonImage.GetFullFilePathExt();
        public string UpdateInvoiceButtonImage => _updateInvoiceButtonImage.GetFullFilePathExt();
        public string DeleteInvoiceButtonImage => _deleteInvoiceButtonImage.GetFullFilePathExt();

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
            InvoiceCreated += SetAddInvoiceStatus;
            InvoiceUpdated += SetAddInvoiceStatus;
            _currencyViewModel.InvoiceNumberViewModel.OnLostFocusEvent += OnInvoiceNumberLostFocus;
            NotPaidInvoiceNumberSelected += LoadNotPaidInvoice;

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
            nameof(_currencyViewModel.InvoiceNumberViewModel.HasErrors),
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

        public void SelectInvoice(InvoiceViewModel invoiceViewModel)
        {
            _invoiceViewModel = invoiceViewModel;
            OnPropertyChange(nameof(_invoiceViewModel));
        }

        public void UnselectInvoice()
        {
            _invoiceViewModel = null;
            OnPropertyChange(nameof(_invoiceViewModel));
        }

        private async Task LoadNotPaidInvoice(string number)
        {
            _currencyViewModel.InvoiceNumberViewModel.NonPayInvoiceOnLoading(number);
            using var scope = serviceProvider.CreateScope();
            var service = scope.GetService<IInvoiceInDayReportService>();
            var res = await Task.Run(() =>
            {
                return service.LoadNotPaidInvoice(number);
            });
            NonPaidInvoiceLoaded?.Invoke(res);
            _currencyViewModel.InvoiceNumberViewModel.InvoiceNotPaid(number);
            AddButtonColor = _orangeYellowColor;
        }

        private async Task CreateAsync()
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var invoiceService = scope.GetService<IInvoiceInDayReportService>();
                var userService = scope.GetService<UserController>();
                var newInvoice = new InvoiceViewModel()
                {
                    Company = _companiesViewModel.CompaniesViewModel.CompaniesSearchBox.Value.Value,
                    CompanyObject = _companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox.Value.Value,
                    Number = _currencyViewModel.InvoiceNumberViewModel.TextBox,
                    Weight = _currencyViewModel.LabeledStringToDecimalTextBoxViewModel.Weight,
                    Amount = _currencyViewModel.AmountViewModel.Money,
                    Income = _currencyViewModel.IncomeViewModel.Money,
                    PayMethod = _currencyViewModel.PayMethodViewModel.CurrentPayMethod,
                    DayReport = _dayReportViewModel
                };
                newInvoice = await invoiceService.CreateAsync(newInvoice);
                InvoiceCreated?.Invoke(newInvoice);
            }
            catch (InvalidOperationException e)
            {
                new MessageBoxLogger().Log(e, Logger.LogLevel.Information);
            }
            catch (ArgumentNullException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }

        }

        private async Task UpdateAsync()
        {
            try
            {
                if (_invoiceViewModel == null) throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Core.ViewModels.InvoiceViewModel)));
                _invoiceViewModel.Company = _companiesViewModel.CompaniesViewModel.CompaniesSearchBox.Value.Value;
                _invoiceViewModel.CompanyObject = _companiesViewModel.CompanyObjectsViewModel.CompanyObjectsSearchBox.Value.Value;
                _invoiceViewModel.Weight = _currencyViewModel.LabeledStringToDecimalTextBoxViewModel.Weight;
                _invoiceViewModel.Amount = _currencyViewModel.AmountViewModel.Money;
                _invoiceViewModel.Income = _currencyViewModel.IncomeViewModel.Money;
                _invoiceViewModel.PayMethod = _currencyViewModel.PayMethodViewModel.CurrentPayMethod;

                using var scope = serviceProvider.CreateScope();
                var service = scope.GetService<IInvoiceInDayReportService>();
                await service.UpdateAsync(_invoiceViewModel);
                InvoiceUpdated?.Invoke(_invoiceViewModel);
            }
            catch (InvalidOperationException e) 
            {
                new MessageBoxLogger().Log(e, Logger.LogLevel.Information);
            }
            catch (ArgumentNullException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }
        private async Task DeleteAsync()
        {
            try
            {
                var invoiceToDelete = _invoiceViewModel ?? throw new ArgumentNullException(ExceptionMessages.NotFound(nameof(Core.ViewModels.InvoiceViewModel)));
                using var scope = serviceProvider.CreateScope();
                var service = scope.GetService<IInvoiceInDayReportService>();
                await service.DeleteAsync(invoiceToDelete);
                UnselectInvoice();
                InvoiceDeleted?.Invoke(invoiceToDelete);
            }
            catch (ArgumentNullException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
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

        private async Task OnInvoiceNumberChange(string number)
        {
            _isInvoiceNotPay = false;
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
                    _isInvoiceNotPay = true;
                }
            }
            else
            {
                AddButtonColor = _lightGreenColor;
            }
            OnPropertyChange(nameof(OnInvoiceNumberChange));
        }

        private async void SetAddInvoiceStatus(InvoiceViewModel invoiceViewModel)
        {
            if (invoiceViewModel == null) return;

            if (invoiceViewModel.IsBeUnpaid() == false || invoiceViewModel.Amount <= invoiceViewModel.Income)
            {
                _currencyViewModel.InvoiceNumberViewModel.InvoiceHasPaid(invoiceViewModel.Number);
                OnPropertyChange(nameof(OnInvoiceNumberChange));
            }
            else
            {
                await OnInvoiceNumberChange(invoiceViewModel.Number);
            }
        }

        private void OnInvoiceNumberLostFocus(string number)
        {
            if (_isInvoiceNotPay)
            {
                NotPaidInvoiceNumberSelected?.Invoke(number);
            }
            else
            {
                _currencyViewModel.IncomeViewModel.SetMaxCurrencyValue(_currencyViewModel.AmountViewModel.Money);
            }
        }
    }
}
