using DelitaTrade.Common.Enums;

namespace DelitaTrade.Core.ViewModels.InvoiceModels
{
    public class InvoiceInDayReportAdvanceViewModel
    {
        public int Id { get; set; }
        public int DayReportId { get; set; }
        public int DeliveryId { get; set; }

        public required string InvoiceNumber { get; set; }

        public decimal Amount { get; set; }

        public decimal Paid { get; set; }
        public decimal Balance { get; set; }
        public PayMethod PayMethod { get; set; }
    }
}
