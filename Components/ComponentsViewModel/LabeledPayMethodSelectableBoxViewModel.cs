using DelitaTrade.ViewModels;
using DelitaTrade.Common.Enums;
using static DelitaTrade.Common.Enums.EnumTranslators.PayMethodTranslator;
using static DelitaTrade.Common.GlobalVariables;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledPayMethodSelectableBoxViewModel : LabeledStringTextBoxViewModel
    {
        private readonly List<string> _payMethods = new();

        private PayMethod _payMethodText = PayMethod.Bank;

        public LabeledPayMethodSelectableBoxViewModel()
        {
            SetAllPayMethods();
        }

        public IEnumerable<string> PayMethods => _payMethods;
        public PayMethod PayMethodsToEnum => _payMethodText;
        public override string TextBox
        {
            get => GetStringValue(Language, _payMethodText);
            set
            {
                _payMethodText = GetPayMethod(Language, value);
                OnPropertyChange();
            }
        }

        private void SetAllPayMethods()
        {
            foreach (var payMethod in PayMethodsToString[Language])
            {
                _payMethods.Add(payMethod.Value);
            }
        }
    }
}
