using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponetsViewModel
{
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
    }
}
