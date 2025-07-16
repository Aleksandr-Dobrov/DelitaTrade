using System.ComponentModel.DataAnnotations;
using static DelitaTrade.Common.ValidationConstants;


namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class BaseInvoiceInputModel
    {
        public int DeliveryId { get; set; }
        [Required]
        [MaxLength(InputInvoiceNumberMaxLength)]
        [MinLength(InvoiceNumberMinLength)]
        public string Number { get; set; } = null!;

        public int CompanyObjectId { get; set; }

        [MaxLength(CompanyObjectNameMaxLength)]
        public required string CompanyObject { get; set; }
        public bool IsBank { get; set; }

        public decimal Amount { get; set; }
    }
}
