using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class PayMethodViewModel
    {
        public int Id => (int)InvoiceType;
        public required PayMethod InvoiceType { get; set; }

        public string Name => InvoiceType.Translate();
    }
}
