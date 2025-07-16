using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.Core.ViewModels.InvoiceModels
{
    public class InvoiceAdvanceMethodViewModel
    {
        public int Id => (int)PayMethod;

        public required InvoiceAdvancePayMethods PayMethod { get; set; }

        public string Name => PayMethod.Translate();
    }
}
