using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Models.DataProviders
{
    public class CurrencyProvider
    {
        const decimal value = 1.5m;

        public char GetDoubleSeparator()
        {
            return value.ToString()[1];
        }

        public char GetCurrencySeparator()
        {
            return $"{value:C2}"[1];
        }

        public int GetCurrencyLength()
        {
            string amount = $"{value:C2}";
            return amount.Length - 4;
        }

        public decimal GetDecimalValue(string currency)
        {
            if (decimal.TryParse(currency,out decimal dec))
            {
                currency = $"{dec:C2}";
            }
            char separator = ' ';
            int indexOfSeparator = -1;
            int indexOfCurrency = -1;

            for (int i = 0; i < currency.Length; i++)
            {
                if (char.IsDigit(currency[i]) == false  && char.IsWhiteSpace(currency[i]) == false && currency[i] != '-')
                {
                    separator = currency[i];
                    indexOfSeparator = i;
                    break;
                }
            }
            
            for (int i = currency.Length - 1; i > 0; i--)
            { 
                if (char.IsDigit(currency[i]))
                {
                    indexOfCurrency = i + 1;
                    break;
                }
            }

            if (indexOfCurrency == -1 || indexOfSeparator == -1)
            {
                throw new InvalidOperationException($"{currency} is incorrect format");
            }

            string amount = currency.Substring(0, indexOfCurrency);
            decimal result;

            if (decimal.TryParse(amount.Replace(separator, GetDoubleSeparator()), out result))
            {
                return result;
            }
            else
            {
                throw new InvalidOperationException($"{currency} can`t be parse");
            }
        }
    }
}

