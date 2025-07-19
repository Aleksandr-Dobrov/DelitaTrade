using DelitaTrade.Common.Enums;
using DelitaTrade.Core.ViewModels.DayReportModels;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels.InvoiceModels
{
    public class InvoiceInDayReportAdvanceInputModel : InvoiceInDayReportAdvanceViewModel
    {
        [Range(0, 999999.99)]
        public decimal Income { get; set; }

        [Range(1, 4)]
        public int ReasonId { get; set; }

        public InvoiceAdvancePayMethods Reason => (InvoiceAdvancePayMethods)ReasonId;

        public IEnumerable<InvoiceAdvanceMethodViewModel> Reasons { get; } =
        [            
            new (){ PayMethod = InvoiceAdvancePayMethods.Partial },
            new (){ PayMethod = InvoiceAdvancePayMethods.NotPay },
            new (){ PayMethod = InvoiceAdvancePayMethods.ForCreditNote },
            new (){ PayMethod = InvoiceAdvancePayMethods.Cancelation },
        ];

        [Range(1, 2)]
        public int PaymentTypeId { get; set; }

        public PaymentType PaymentType => (PaymentType)PaymentTypeId;

        public IEnumerable<PaymentTypeViewModel> PaymentTypes { get; } =
        [
            new (){ PaymentType = PaymentType.Cash },
            new (){ PaymentType = PaymentType.Bank },
            new (){ PaymentType = PaymentType.Card }
        ];
    }
}
