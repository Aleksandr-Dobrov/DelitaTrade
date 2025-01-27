using DelitaTrade.ViewModels;
using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.Enums.EnumTranslators.PayMethodTranslator;
using static DelitaTrade.Common.GlobalVariables;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledPayMethodSelectableBoxViewModel : LabeledStringTextBoxViewModel
    {
        private readonly List<string> _payMethods = new();

        private PayMethod _payMethodText = PayMethod.Bank;

        public LabeledPayMethodSelectableBoxViewModel()
        {
            SetAllPayMethods();
            PropertyChanged += OnViewModelChange;
            PayMethodChange += (p) => { };
        }
        public event Action<PayMethod> PayMethodChange;

        public IEnumerable<string> PayMethods => _payMethods;
        public PayMethod CurrentPayMethod => _payMethodText;
        public override string TextBox
        {
            get => GetStringValue(Language, _payMethodText);
            set
            {
                _payMethodText = GetPayMethod(Language, value);
                OnPropertyChange();
            }
        }

        public void SetPayMethod(PayMethod payMethod)
        {
            TextBox = PayMethodsToString[Language][payMethod];
        }

        private void SetAllPayMethods()
        {
            foreach (var payMethod in PayMethodsToString[Language])
            {
                _payMethods.Add(payMethod.Value);
            }
        }

        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TextBox))
            {
                PayMethodChange(CurrentPayMethod);
            }
        }
    }
}
