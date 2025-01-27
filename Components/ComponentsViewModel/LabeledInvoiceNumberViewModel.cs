using DelitaTrade.Common.DelitaValidations;
using static DelitaTrade.Common.DelitaAppConstants;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledInvoiceNumberViewModel : LabeledStringTextBoxViewModel
    {        
        private string _invoiceNumber = DefaultInvoiceNumber;
        [IsDigitsValidation(10)]
        public override string TextBox 
        {
            get => _invoiceNumber; 
            set
            {
                if (CheckInvoiceIdIsValid(value))
                {
                    _invoiceNumber = value;
                    OnPropertyChange();
                }
            }
        }
        private bool CheckInvoiceIdIsValid(string id)
        {
            return id.Length == 10 && id.All(char.IsDigit);
        }
    }
}
