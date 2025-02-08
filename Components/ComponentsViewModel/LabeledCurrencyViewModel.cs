using DelitaTrade.Models.DataProviders;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            }
        }

        public void SetCurrencyValue(string value)
        {
            TextBox = value;
        }

        public void SetCurrencyValue(decimal value)
        {
            TextBox = $"{value:C2}";
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
