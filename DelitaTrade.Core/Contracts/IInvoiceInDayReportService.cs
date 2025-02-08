using DelitaTrade.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Contracts
{
    public interface IInvoiceInDayReportService
    {
        Task<IEnumerable<InvoiceViewModel>> AllReadonlyAsync();
        Task<IEnumerable<InvoiceViewModel>> SearchReadonlyAsync(string arg, int limit);
        Task<IEnumerable<InvoiceViewModel>> AllInDayReportAsync(int dayReportId);
        Task<InvoiceViewModel> LoadNotPaidInvoice(string number);
        Task<InvoiceViewModel> CreateAsync(InvoiceViewModel newInvoice);
        Task UpdateAsync(InvoiceViewModel invoice);
        Task DeleteAsync(InvoiceViewModel invoice);
    }
}
