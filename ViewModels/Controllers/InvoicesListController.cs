using DelitaTrade.Core.ViewModels;
using DelitaTrade.WpfViewModels;
using System.Collections.ObjectModel;

namespace DelitaTrade.ViewModels.Controllers
{
    public class InvoicesListController
    {
        private ObservableCollection<WpfInvoiceListViewModel> _invoicesListViewModel = new();

        private WpfInvoiceListViewModel? _selectedInvoice;

        public event Action<InvoiceViewModel>? InvoiceSelected;
        public event Action? InvoiceUnSelected;

        public WpfInvoiceListViewModel? SelectedInvoiceViewModel
        {
            get => _selectedInvoice;
            set
            {
                _selectedInvoice = value;
                if (_selectedInvoice != null)
                {
                    InvoiceSelected?.Invoke(_selectedInvoice.InvoiceViewModel);
                }
                else
                {
                    InvoiceUnSelected?.Invoke();
                }
            }
        }

        public ObservableCollection<WpfInvoiceListViewModel> InvoicesListViewModel => _invoicesListViewModel;
               
        public void AddInvoice(InvoiceViewModel invoice)
        {
            _invoicesListViewModel.Add(new WpfInvoiceListViewModel(invoice));            
        }

        public void DeleteInvoice(InvoiceViewModel invoice)
        {
            _invoicesListViewModel.Remove(new WpfInvoiceListViewModel(invoice));
        }

        public void UpdateInvoice(InvoiceViewModel invoice)
        {
            _invoicesListViewModel.FirstOrDefault(i => i.Id == invoice.IdInDayReport)?.OnViewModelChange();
        }

        public void OnDayReportSelected(DayReportViewModel dayReportViewModel)
        {
            _invoicesListViewModel.Clear();
            if (dayReportViewModel != null)
            {
                if (dayReportViewModel.Invoices.Count > 0) 
                {
                    foreach (var invoice in dayReportViewModel.Invoices.OrderBy(i => i.PayMethod)
                                                              .ThenByDescending(i => i.Income == 0 && i.PayMethod != Common.Enums.PayMethod.Bank)
                                                              .ThenBy(i => i.Amount == 0)
                                                              .ThenBy(i => i.Number)
                                                              .ThenBy(i => i.CompanyObject.Name))
                    {
                        AddInvoice(invoice);
                    }
                }
            }
        }

        public void OnDayReportUnSelect()
        {
            _invoicesListViewModel.Clear();
        }
    }
}
