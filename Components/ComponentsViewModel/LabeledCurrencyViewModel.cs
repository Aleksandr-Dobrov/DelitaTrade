using DelitaTrade.Models.DataProviders;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public enum CurrencyStatus
    {
        Positive,
        Negative,
        Zero
    }

    public class LabeledCurrencyViewModel : LabeledStringTextBoxViewModel
    {
        private CurrencyProvider _currencyProvider = new();
        private decimal _money;
        private bool _isLoaded;
        private decimal _maxCurrency = decimal.MaxValue;
        public LabeledCurrencyViewModel()
        {
            PropertyChanged += OnViewModelChanged;
            CurrencyChanged += (s) => { };
        }

        public event Action<string> CurrencyChanged;
        public CurrencyStatus CurrencyStatus { get; set; }

        public decimal Money => _money;

        public override string TextBox 
        {
            get => $"{_money:C2}";
            set
            {
                _money = _currencyProvider.GetDecimalValue(value);                
                SetCurrency();
                OnPropertyChange();
                Validate($"Max value is {_maxCurrency:C2}", _money <= _maxCurrency);               
            }
        }

        public void SetCurrencyValue(string value)
        {
            if (_isLoaded)
            {
                _isLoaded = false;
            }
            else
            {
                TextBox = value;
            }
        }

        public void SetMaxAvailableCurrencyValue(string value)
        {
            if (_isLoaded)
            {
                _isLoaded = false;
            }
            else
            {
                if (_maxCurrency == decimal.MaxValue)
                {
                    TextBox = value;
                }
                else
                {
                    TextBox = $"{_maxCurrency:C2}";                    
                }
            }
        }

        public void SetCurrencyValue(decimal value)
        {
            TextBox = $"{value:C2}";
        }

        public void SetLoadedCurrencyValue(decimal value)
        {
            _isLoaded = true;
            SetCurrencyValue(value);
        }

        public void SetMaxCurrencyValue(decimal value)
        {
            _maxCurrency = value;
        }

        public void ResetMaxValue()
        {
            _maxCurrency = decimal.MaxValue;
            Validate(_money <= _maxCurrency, nameof(TextBox));
        }

        private void OnViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TextBox))
            {
                CurrencyChanged(TextBox);
            }
        }

        private void SetCurrency()
        {
            if (CurrencyStatus == CurrencyStatus.Positive)
            {
                if (_money < 0)
                {
                    _money = 0;
                }
            }
            else if(CurrencyStatus == CurrencyStatus.Negative)
            {
                if (_money > 0)
                {
                    _money *= -1;
                }
            }
            else if (CurrencyStatus == CurrencyStatus.Zero)
            {
                _money = 0;
            }
        }
    }
}
