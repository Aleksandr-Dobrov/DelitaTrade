using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class InvoiceInputViewModel
    {
        private readonly InvoiceCompaniesInputViewModel _invoiceCompanyInputViewModel;
        private readonly InvoiceCurrencyInputViewModel _invoiceCurrencyInputViewModel;

        public InvoiceInputViewModel(InvoiceCompaniesInputViewModel invoiceCompanyInputViewModel, InvoiceCurrencyInputViewModel invoiceCurrencyInputViewModel)
        {
            _invoiceCompanyInputViewModel = invoiceCompanyInputViewModel;
            _invoiceCurrencyInputViewModel = invoiceCurrencyInputViewModel;
            _invoiceCompanyInputViewModel.OnCompanyObjectIsBankChange += OnCompanyObjectIsBankChange;
            
        }

        public InvoiceCompaniesInputViewModel InvoiceCompanyInputViewModel => _invoiceCompanyInputViewModel;

        public InvoiceCurrencyInputViewModel InvoiceCurrencyInputViewModel => _invoiceCurrencyInputViewModel;

        private void OnCompanyObjectIsBankChange(ICompanyObjectIsBankPay objectIsBankPay)
        {
            if (objectIsBankPay.IsBankPay) InvoiceCurrencyInputViewModel.SetPayMethod(PayMethod.Bank);
            else InvoiceCurrencyInputViewModel.SetPayMethod(PayMethod.Cash);
        }
    }
}
