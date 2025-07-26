using DelitaTrade.Common.Enums;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels.InvoiceModels
{
    public class InvoiceInDayReportAdvanceInputModel : InvoiceInDayReportAdvanceViewModel
    {
        [Range(0, 999999.99)]
        public decimal Income { get; set; }
                
        public int ReasonId { get; set; }

        public InvoiceAdvancePayMethods Reason => (InvoiceAdvancePayMethods)ReasonId;

        public IEnumerable<InvoiceAdvanceMethodViewModel> Reasons { get; set; } = new List<InvoiceAdvanceMethodViewModel>();

        public int PaymentTypeId { get; set; }

        public PayMethod PaymentType => (PayMethod)PaymentTypeId;

        public IEnumerable<PayMethodViewModel> PaymentTypes { get; set; } = new List<PayMethodViewModel>();
    }
}
