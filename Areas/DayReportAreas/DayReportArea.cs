using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.Areas.DayReportAreas
{
    public class DayReportArea : ViewModelBase
    {
        private readonly InvoiceInputViewModel _invoiceInputViewModel;
        private readonly DayReportLoaderViewModel _dayReportLoaderViewModel;
        private readonly InvoicesListController _invoicesListController;
        private readonly AdvanceViewModel _advancedViewModel;

        public DayReportArea(InvoiceInputViewModel invoiceInputViewModel, DayReportLoaderViewModel dayReportLoaderViewModel, InvoicesListController invoicesListController, AdvanceViewModel advancedViewModel)
        {
            _invoiceInputViewModel = invoiceInputViewModel;
            _dayReportLoaderViewModel = dayReportLoaderViewModel;
            _invoicesListController = invoicesListController;
            _advancedViewModel = advancedViewModel;
            DayReportLoaderViewModel.DayReportSelected += _invoiceInputViewModel.OnLoadedDayReport;
            DayReportLoaderViewModel.DayReportUnSelect += _invoiceInputViewModel.OnUnSelectDayReport;
            InvoiceInputViewModel.InvoiceCreated += InvoicesListController.AddInvoice;
            InvoiceInputViewModel.InvoiceDeleted += InvoicesListController.DeleteInvoice;
            InvoiceInputViewModel.InvoiceUpdated += InvoicesListController.UpdateInvoice;
            DayReportLoaderViewModel.DayReportSelected += InvoicesListController.OnDayReportSelected;
            DayReportLoaderViewModel.DayReportUnSelect += InvoicesListController.OnDayReportUnSelect;
            InvoicesListController.InvoiceSelected += InvoiceInputViewModel.OnSelectInvoice;
            InvoicesListController.InvoiceUnSelected += InvoiceInputViewModel.OnUnSelectInvoice;
            InvoiceInputViewModel.InvoiceCreated += DayReportLoaderViewModel.DayReportUpdate;
            InvoiceInputViewModel.InvoiceDeleted += DayReportLoaderViewModel.DayReportUpdate;
            InvoiceInputViewModel.InvoiceUpdated += DayReportLoaderViewModel.DayReportUpdate;
            DayReportLoaderViewModel.DayReportSelected += AdvancedViewModel.OnDayReportSelected;
            DayReportLoaderViewModel.DayReportUnSelect += AdvancedViewModel.OnDayReportUnselected;
            AdvancedViewModel.DayReportChange += DayReportLoaderViewModel.DayReportUpdate;
        }

        public InvoiceInputViewModel InvoiceInputViewModel => _invoiceInputViewModel;

        public DayReportLoaderViewModel DayReportLoaderViewModel => _dayReportLoaderViewModel;

        public InvoicesListController InvoicesListController => _invoicesListController;

        public AdvanceViewModel AdvancedViewModel => _advancedViewModel;
    }
}
