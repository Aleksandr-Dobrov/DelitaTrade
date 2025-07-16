using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.InvoiceModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IInvoiceInDayReportService
    {
        Task<IEnumerable<InvoiceViewModel>> AllReadonlyAsync();
        Task<IEnumerable<InvoiceViewModel>> SearchReadonlyAsync(string arg, int limit);
        Task<IEnumerable<InvoiceViewModel>> AllInDayReportAsync(int dayReportId);
        Task<InvoiceInDayReportAdvanceViewModel?> GetAdvanceByIdAsync(int id);
        Task<IEnumerable<InvoiceViewModel>> GetByIdAsync(IEnumerable<int> ids);
        Task<InvoiceViewModel> LoadNotPaidInvoiceAsync(string number);
        Task<InvoiceViewModel> CreateAsync(InvoiceViewModel newInvoice);
        Task UpdateAsync(InvoiceViewModel invoice);
        Task DeleteAsync(InvoiceViewModel invoice);
        Task AdvancePayAsync(UserViewModel user, InvoiceInDayReportAdvanceInputModel payment, int deliveryId);
        Task CompleteAsync(UserViewModel user, PaymentCompleteInputModel payment);
        Task<bool> IsBankPayAsync(int id);
    }
}
