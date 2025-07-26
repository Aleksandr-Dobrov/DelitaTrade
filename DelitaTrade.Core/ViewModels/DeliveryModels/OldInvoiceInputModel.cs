using DelitaTrade.Common.Enums;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class OldInvoiceInputModel : BaseInvoiceInputModel
    {       
        public int PaymentTypeId { get; set; }
        public PayMethod PaymentType => (PayMethod)PaymentTypeId;

        public IEnumerable<PayMethodViewModel> PaymentTypes { get; } =
        [
            new () { InvoiceType = PayMethod.OldPayCash },
            new () { InvoiceType = PayMethod.OldPayCard }
        ];
    }
}
