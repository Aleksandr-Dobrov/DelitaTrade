using DelitaTrade.Common.DelitaValidations;
using DelitaTrade.ViewModels.Interfaces;
using System.ComponentModel;
using static DelitaTrade.Common.DelitaAppConstants;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledInvoiceNumberViewModel : LabeledStringTextBoxViewModel
    {        
        private string _invoiceNumber = DefaultInvoiceNumber;
        private string _lastInvoiceNumber = DefaultInvoiceNumber;
        private bool IsLastNumberSet = true;

        public LabeledInvoiceNumberViewModel()
        {
            PropertyChanged += OnViewModelChange;
            OnInvoiceNumberChanged += (i) => { };
        }

        public event Action<string> OnInvoiceNumberChanged;

        [IsDigitsValidation(10)]
        public override string TextBox 
        {
            get => _invoiceNumber; 
            set
            {
                if (CheckInvoiceIdIsValid(value))
                {
                    _invoiceNumber = value;
                    if (IsLastNumberSet) _lastInvoiceNumber = value;
                    OnPropertyChange();
                }
            }
        }

        public void SetLastNumber()
        {
            TextBox = _lastInvoiceNumber;
            IsLastNumberSet = true;
        }

        public void InvoiceHasPaid(string number)
        {
            AddError(nameof(TextBox), $"The invoice - {number} has been paid");
        }
        private bool CheckInvoiceIdIsValid(string id)
        {
            return id.Length == 10 && id.All(char.IsDigit);
        }

        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TextBox))
            {
                OnInvoiceNumberChanged(TextBox);
            }
        }

        public void SetExpenseNumber(DateTime date)
        {
            _lastInvoiceNumber = TextBox;
            IsLastNumberSet = false;
            TextBox = $"0{date.Date:ddMMyyyy}{0}";
        }
    }
}
