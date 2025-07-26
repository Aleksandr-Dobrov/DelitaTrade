using DelitaTrade.Commands;
using DelitaTrade.Common.DelitaValidations;
using DelitaTrade.ViewModels.Interfaces;
using System.ComponentModel;
using System.Windows.Input;
using static DelitaTrade.Common.DelitaAppConstants;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledInvoiceNumberViewModel : LabeledStringTextBoxViewModel
    {
        private bool _newValueIsNotEqual;
        private string _invoiceNumber = DefaultInvoiceNumber;
        private string _lastInvoiceNumber = DefaultInvoiceNumber;
        private bool IsLastNumberSet = true;

        public LabeledInvoiceNumberViewModel()
        {
            PropertyChanged += OnViewModelChange;
            OnInvoiceNumberChanged += async (i) => { await new Task(() => { }); };
            OnLostFocusEvent += (s) => { };
            LostFocus = new NotAsyncDefaultCommand(OnLostFocus);
        }

        public event Func<string, Task> OnInvoiceNumberChanged;
        public event Action<string> OnLostFocusEvent;
        public ICommand LostFocus { get; }

        [IsDigitsValidation(10)]
        public override string TextBox 
        {
            get => _invoiceNumber; 
            set
            {
                if (CheckInvoiceIdIsValid(value))
                {
                    _newValueIsNotEqual = _invoiceNumber != value;
                    _invoiceNumber = value;
                    if (IsLastNumberSet) _lastInvoiceNumber = value;
                    if (_newValueIsNotEqual) OnPropertyChange();
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

        public void InvoiceNotPaid(string number)
        {
            ClearErrors(nameof(TextBox));
        }

        public void NonPayInvoiceOnLoading(string number)
        {
            AddError(nameof(TextBox), $"Invoice : {number} is not loading");
        }

        public void SetExpenseNumber(DateTime date)
        {
            _lastInvoiceNumber = TextBox;
            IsLastNumberSet = false;
            TextBox = $"0{date.Date:ddMMyyyy}{0}";
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

        private void OnLostFocus()
        {
            OnLostFocusEvent?.Invoke(TextBox);
        }
    }
}
