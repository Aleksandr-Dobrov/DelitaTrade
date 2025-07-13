namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class DeliveryInputModel
    {
        public int DayReportId { get; set; }
        public int DeliveryAddressId { get; set; }
        IList<InvoiceInputModel> Invoices { get; set; } = new List<InvoiceInputModel>();
    }
}
