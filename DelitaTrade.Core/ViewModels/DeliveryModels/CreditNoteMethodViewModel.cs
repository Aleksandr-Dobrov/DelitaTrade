using DelitaTrade.Common.Enums;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class CreditNoteMethodViewModel
    {
        public int Id => (int)CreditNoteType;
        public required CreditNoteMethod CreditNoteType { get; set; }

        public string Name => CreditNoteType.Translate();
    }
}
