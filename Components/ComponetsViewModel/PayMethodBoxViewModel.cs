using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class PayMethodBoxViewModel : ViewModelBase
    {
        private readonly string[] _payMethods = ["Банка", "В брой", "С карта", "За кредитно", "За анулиране",  "Кредитно", "Стара сметка", "Разход"];

        public string[] PayMethods => _payMethods;

        private string _payMethodText = "Банка";

        public string PayMethodText
        {
            get => _payMethodText;
            set
            { 
                _payMethodText = value;
                OnPropertyChange(nameof(PayMethodText));
            }
        }

    }
}
