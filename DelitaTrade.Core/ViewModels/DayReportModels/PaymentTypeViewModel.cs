using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class PaymentTypeViewModel
    {
        public int Id => (int)PaymentType;
        public PaymentType PaymentType {  get; set; }
        public string Name => PaymentType.Translate();
    }
}
