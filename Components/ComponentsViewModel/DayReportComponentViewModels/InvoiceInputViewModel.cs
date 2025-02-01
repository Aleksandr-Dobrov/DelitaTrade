using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Services;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using System.ComponentModel.DataAnnotations;

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
           OnLoadedDayReport(new DayReportViewModel()
            {
                Date = DateTime.Now,
                Banknotes = new(),
                User = new UserViewModel() { }
            });
            _invoiceInputCommandsViewModel.SelectInvoice(new Core.ViewModels.InvoiceViewModel()
            {
                Company = new Core.ViewModels.CompanyViewModel()
                {
                    Name = "new"
                },
                CompanyObject = new Core.ViewModels.CompanyObjectViewModel()
                {
                    Name = "obj",
                    Company = new Core.ViewModels.CompanyViewModel()
                    {
                        Name = "new"
                    }
                },
                Number = "1000000005"
            });
            
        }        
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

        private void OnCompanyObjectIsBankChange(ICompanyObjectIsBankPay objectIsBankPay)
        {
            if (objectIsBankPay.IsBankPay) InvoiceCurrencyInputViewModel.SetPayMethod(PayMethod.Bank);
            else InvoiceCurrencyInputViewModel.SetPayMethod(PayMethod.Cash);
        }

        private void OnInvoiceCreated(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _soundService.PlaySound(Models.Configurations.SoundEfect.AddInvoice);
        }
    }
}
