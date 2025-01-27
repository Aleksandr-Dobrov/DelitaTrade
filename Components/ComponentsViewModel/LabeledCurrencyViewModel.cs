using DelitaTrade.Models.DataProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledCurrencyViewModel : LabeledStringTextBoxViewModel
    {
        private CurrencyProvider _currencyProvider = new();
        private decimal _money;
        public override string TextBox 
        {
            get => $"{_money:C2}";
            set
            {
                _money = _currencyProvider.GetDecimalValue(value);
                OnPropertyChange();
            }
        }
    }
}
