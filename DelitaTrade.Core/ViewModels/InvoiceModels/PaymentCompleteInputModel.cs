using DelitaTrade.Common.Enums;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Core.ViewModels.DeliveryModels;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels.InvoiceModels
{
    public class PaymentCompleteInputModel
    {
        public int Id { get; set; }

        public int DeliveryId { get; set; }
                
        public int PaymentTypeId { get; set; }

        public PayMethod PaymentType => (PayMethod)PaymentTypeId;

        public IEnumerable<PayMethodViewModel> PaymentTypes { get; set; } = new List<PayMethodViewModel>();
    }
}
