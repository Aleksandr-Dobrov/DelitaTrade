using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using System.ComponentModel;
using System.Windows;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledWeightTextBoxViewModel : LabeledStringTextBoxViewModel
    {        
        private const string _unit = "kg.";
        private decimal _weight;
        private DayReportInputOptionsViewModelComponent _options;

        public LabeledWeightTextBoxViewModel(DayReportInputOptionsViewModelComponent options)
        {
            _options = options;
            Label = "Weight";
        }

        public decimal Weight => _weight;

        //TODO Create validation attribute for positive number
        public override string TextBox 
        {
            get => $"{_weight:F2} {_unit}";
            set
            {
                if (value != null && value.Contains(_unit))
                {
                    value = value.Replace(_unit, "").Trim();
                }

                if(decimal.TryParse(value, out _weight))
                {
                    OnPropertyChange();
                }
            }
        }

        public override bool IsEnable 
        {
            get => _options.WeightIsOn; 
            set
            {
                _options.WeightIsOn = value;
                OnPropertyChange();
            }
        }
        public override Visibility VisibilityProperty => _options.Visibility;
    }
}
