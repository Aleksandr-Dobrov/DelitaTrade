using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IDeliveryService
    {
        Task AddDeliveryAsync(DeliveryInputModel deliveryInput);
        Task AddRangeDeliveryAsync(IEnumerable<DeliveryInputModel> invoices);
        Task AddPaymentAsync(InvoiceViewModel invoice);
    }
}
