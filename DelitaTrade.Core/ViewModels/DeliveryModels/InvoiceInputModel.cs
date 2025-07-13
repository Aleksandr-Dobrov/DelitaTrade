using System.ComponentModel.DataAnnotations;
using static DelitaTrade.Common.ValidationConstants;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class InvoiceInputModel
    {
        [MaxLength(InvoiceNumberMaxLength)]
        public required string Number { get; set; }

        public int CompanyId { get; set; }
        public required string Company { get; set; }
        public int CompanyObjectId { get; set; }

        [MaxLength(CompanyObjectNameMaxLength)]
        public required string CompanyObject { get; set; }

        public decimal Amount { get; set; }
        public decimal Weight { get; set; }
    }
}
