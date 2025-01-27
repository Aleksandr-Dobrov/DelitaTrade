using DelitaTrade.Common.Enums;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponentsViewModel
{
    //Deprecated
    public class PayMethodBoxViewModel : ViewModelBase
    {
        private readonly string[] _payMethods = ["Банка", "В брой", "С карта", "За кредитно", "За анулиране",  "Кредитно", "Стара сметка", "Разход"];

        private string _payMethodText = "Банка";

        public string[] PayMethods => _payMethods;

        public string PayMethodText
        {
            get => _payMethodText;
            set
            { 
                _payMethodText = value;
                OnPropertyChange(nameof(PayMethodText));
            }
        }
        private PayMethod _payMethod = PayMethod.Bank;
        public string[] PayMethodsEnum = [PayMethod.Bank.ToString(), PayMethod.Card.ToString(), PayMethod.OldPayCard.ToString()];
        public string PayMethodE
        {
            get => _payMethod.ToString();
            set
            {
                _payMethod = Enum.Parse<PayMethod>(value);
                OnPropertyChange();
            }
        }
    }
}
