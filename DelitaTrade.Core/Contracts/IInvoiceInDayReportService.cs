using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IInvoiceInDayReportService
    {
        Task<IEnumerable<InvoiceViewModel>> AllReadonlyAsync();
        Task<IEnumerable<InvoiceViewModel>> SearchReadonlyAsync(string arg, int limit);
        Task<IEnumerable<InvoiceViewModel>> AllInDayReportAsync(int dayReportId);
        Task<IEnumerable<InvoiceViewModel>> GetById(IEnumerable<int> ids);
        Task<InvoiceViewModel> LoadNotPaidInvoice(string number);
        Task<InvoiceViewModel> CreateAsync(InvoiceViewModel newInvoice);
        Task UpdateAsync(InvoiceViewModel invoice);
        Task DeleteAsync(InvoiceViewModel invoice);
    }
}
