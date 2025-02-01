using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Areas.DayReportAreas
{
    public class DayReportArea : ViewModelBase
    {
        private readonly InvoiceInputViewModel _invoiceInputViewModel;

        public DayReportArea(InvoiceInputViewModel invoiceInputViewModel)
        {
            _invoiceInputViewModel = invoiceInputViewModel;
        }

        public InvoiceInputViewModel InvoiceInputViewModel => _invoiceInputViewModel;
    }
}
