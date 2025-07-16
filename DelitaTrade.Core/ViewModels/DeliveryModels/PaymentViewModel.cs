using DelitaTrade.Common.Enums;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        public required string CompanyName { get; set; }

        public required string CompanyObjectName { get; set; }

        public required string InvoiceNumber { get; set; }

        public decimal Amount { get; set; }

        public decimal Balance { get; set; }

        public decimal Income { get; set; }

        public PayMethod PayMethod { get; set; }

        public decimal Weight { get; set; }
        
        public bool IsBank { get; set; }

        public bool IsCompleted { get; set; }
    }
}
