using DelitaTrade.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class CreditNoteInputModel : BaseInvoiceInputModel
    {
        public decimal Income { get; set; }

        [Range(1, 3)]
        public int CreditNoteTypeId { get; set; }

        public CreditNoteMethod CreditNoteType => (CreditNoteMethod)CreditNoteTypeId;

        public IEnumerable<CreditNoteMethodViewModel> CreditNoteTypes { get; } = 
            [
                new() { CreditNoteType = CreditNoteMethod.NotDeducted },
                new() { CreditNoteType = CreditNoteMethod.Deducted },
                new() { CreditNoteType = CreditNoteMethod.Bank }
            ];
    }
}
