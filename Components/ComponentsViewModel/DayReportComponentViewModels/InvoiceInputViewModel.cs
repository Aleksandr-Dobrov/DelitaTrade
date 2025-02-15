using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Services;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class InvoiceInputViewModel : ViewModelBase
    {
        private bool _isEditable;
        private readonly InvoiceCompaniesInputViewModel _invoiceCompanyInputViewModel;
        private readonly InvoiceCurrencyInputViewModel _invoiceCurrencyInputViewModel;
        private readonly InvoiceInputCommandsViewModel _invoiceInputCommandsViewModel;
        private readonly DelitaSoundService _soundService;

        public InvoiceInputViewModel(InvoiceCompaniesInputViewModel invoiceCompanyInputViewModel, InvoiceCurrencyInputViewModel invoiceCurrencyInputViewModel, InvoiceInputCommandsViewModel invoiceInputCommandsViewModel, DelitaSoundService soundService)
        {
            _soundService = soundService;
            _invoiceCompanyInputViewModel = invoiceCompanyInputViewModel;
            _invoiceCurrencyInputViewModel = invoiceCurrencyInputViewModel;
            _invoiceCompanyInputViewModel.OnCompanyObjectIsBankChange += OnCompanyObjectIsBankChange;
            _invoiceInputCommandsViewModel = invoiceInputCommandsViewModel;
            _invoiceInputCommandsViewModel.InitializedCommands(InvoiceCompanyInputViewModel, InvoiceCurrencyInputViewModel);
            _invoiceInputCommandsViewModel.InvoiceCreated += OnInvoiceCreated;
            _invoiceInputCommandsViewModel.InvoiceDeleted += OnInvoiceDeleted;
            InvoiceInputCommandsViewModel.NonPaidInvoiceLoaded += OnLoadInvoice;
            InvoiceInputCommandsViewModel.InvoiceUpdated += OnInvoiceUpdated;
        }

        public event Action<Core.ViewModels.InvoiceViewModel>? InvoiceCreated;
        public event Action<Core.ViewModels.InvoiceViewModel>? InvoiceDeleted;
        public event Action<Core.ViewModels.InvoiceViewModel>? InvoiceUpdated;
        public InvoiceCompaniesInputViewModel InvoiceCompanyInputViewModel => _invoiceCompanyInputViewModel;

        public InvoiceCurrencyInputViewModel InvoiceCurrencyInputViewModel => _invoiceCurrencyInputViewModel;

        public InvoiceInputCommandsViewModel InvoiceInputCommandsViewModel => _invoiceInputCommandsViewModel;

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChange();
            }
        }

        public void OnLoadedDayReport(DayReportViewModel dayReportViewModel)
        {
            _invoiceInputCommandsViewModel.SelectDayReport(dayReportViewModel);
            IsEditable = true;
        }

        public void OnUnSelectDayReport()
        {
            _invoiceInputCommandsViewModel.UnSelectDayReport();
            IsEditable = false;
        }
        
        public void OnSelectInvoice(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            InvoiceInputCommandsViewModel.SelectInvoice(invoiceViewModel);
            InvoiceCompanyInputViewModel.OnSelectedInvoice(invoiceViewModel);
            InvoiceCurrencyInputViewModel.OnInvoiceSelected(invoiceViewModel);
        }
        public void OnLoadInvoice(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            InvoiceCompanyInputViewModel.OnLoadedInvoice(invoiceViewModel);
            InvoiceCurrencyInputViewModel.AmountViewModel.SetCurrencyValue(invoiceViewModel.Amount);
            InvoiceCurrencyInputViewModel.IncomeViewModel.SetMaxCurrencyValue(invoiceViewModel.Income);
            InvoiceCurrencyInputViewModel.IncomeViewModel.SetLoadedCurrencyValue(invoiceViewModel.Income);
        }

        public void OnUnSelectInvoice()
        {
            InvoiceInputCommandsViewModel.UnselectInvoice();
        }

        private void OnCompanyObjectIsBankChange(ICompanyObjectIsBankPay objectIsBankPay)
        {
            if (objectIsBankPay.IsBankPay) InvoiceCurrencyInputViewModel.SetPayMethod(PayMethod.Bank);
            else InvoiceCurrencyInputViewModel.SetPayMethod(PayMethod.Cash);
        }

        private void OnInvoiceCreated(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _soundService.PlaySound(SoundEfect.AddInvoice);
            InvoiceCreated?.Invoke(invoiceViewModel);
        }

        private void OnInvoiceDeleted(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _soundService.PlaySound(SoundEfect.DeleteInvoice);
            InvoiceDeleted?.Invoke(invoiceViewModel);
        }

        private void OnInvoiceUpdated(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            InvoiceUpdated?.Invoke(invoiceViewModel);
        }
    }
}
