using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Areas.DayReportAreas
{
    public class DayReportArea : ViewModelBase
    {
        private readonly InvoiceInputViewModel _invoiceInputViewModel;
        private readonly DayReportLoaderViewModel _dayReportLoaderViewModel;

        public DayReportArea(InvoiceInputViewModel invoiceInputViewModel, DayReportLoaderViewModel dayReportLoaderViewModel)
        {
            _invoiceInputViewModel = invoiceInputViewModel;
            _dayReportLoaderViewModel = dayReportLoaderViewModel;
            _dayReportLoaderViewModel.DayReportSelected += _invoiceInputViewModel.OnLoadedDayReport;
            _dayReportLoaderViewModel.DayReportUnSelect += _invoiceInputViewModel.UnSelectDayReport;
        }

        public InvoiceInputViewModel InvoiceInputViewModel => _invoiceInputViewModel;

        public DayReportLoaderViewModel DayReportLoaderViewModel => _dayReportLoaderViewModel;
    }
}
