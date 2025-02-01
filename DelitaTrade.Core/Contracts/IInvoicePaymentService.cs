namespace DelitaTrade.Core.Contracts
{
    public interface IInvoicePaymentService
    {
        Task<bool> IsPay(string invoiceNumber);
        Task<bool> IsExists(string invoiceNumber);
    }
}
