using DelitaTrade.Core.Models.ImportModels;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IDeliveryService
    {
        Task<DeliveryViewModel> AddDeliveryAsync(DeliveryInputModel deliveryInput, UserViewModel user);
        Task ImportPaymentsToDayReportAsync(UserViewModel user, int dayReportId, DayReportJsonImportModel dayReportJson);
        Task AddInvoiceAsync(UserViewModel user, InvoiceInputModel invoice, int deliveryId);
        Task AddOldInvoiceAsync(UserViewModel user, OldInvoiceInputModel oldInvoice, int deliveryId);
        Task AddCreditNoteAsync(UserViewModel user, CreditNoteInputModel creditNote, int deliveryId);
        Task<DeliveryViewModel?> GetByIdAsync(UserViewModel user, int deliveryId);
        Task<int?> GetVehicleIdFromDeliveryAsync(int deliveryId);
        Task CompleteAllAsync(UserViewModel user, int deliveryId);
        Task<bool> IsCompleteAsync(UserViewModel user, int deliveryId);
        Task<int> GetDayReportIdAsync(int deliveryId);
        Task DeleteAsync(UserViewModel user, int deliveryId);
    }
}
