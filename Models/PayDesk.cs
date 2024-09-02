using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class PayDesk
    {
        [DataMember]
        Dictionary<decimal, int> _banknotes;

        [DataMember]
        private decimal _amount;
        public PayDesk()
        {
            BanknotesInitialize();            
        }

        public decimal Amount => _amount;

        private void BanknotesInitialize()
        {
            _banknotes = new Dictionary<decimal, int>
            {
                { 0.01m, 0 },
                { 0.02m, 0 },
                { 0.05m, 0 },
                { 0.1m, 0 },
                { 0.2m, 0 },
                { 0.5m, 0 },
                { 1.0m, 0 },
                { 2.0m, 0 },
                { 5.0m, 0 },
                { 10.0m, 0 },
                { 20.0m, 0 },
                { 50.0m, 0 },
                { 100.0m, 0 },
            };
        }

        public void OnBanknoteChanged()
        {
            CalculationAmount();
        }

        private void CalculationAmount()
        {
            _amount = _banknotes.Sum(b => b.Value * b.Key);
        }

        public void AddMoney(string banknote, int count)
        {
            if (decimal.TryParse(banknote, out decimal banknoteM) && _banknotes.ContainsKey(banknoteM))
            {
                if (count > 0)
                {
                    _banknotes[banknoteM] += count;
                    OnBanknoteChanged();
                }
                else
                {
                    throw new ArgumentException("Count of banknotes can`t be negative value!");
                }
            }
            else 
            {
                throw new ArgumentException("Value of banknote is incorrect!");
            }


        }

        public void RemoveMonet(string banknote, int count)
        {
            if (decimal.TryParse(banknote, out decimal banknoteM) && _banknotes.ContainsKey(banknoteM))
            {
                if (count < 0)
                {
                    throw new ArgumentException("Count of banknotes can`t be negative value!");
                }
                else if (count > _banknotes[banknoteM])
                {
                    _banknotes[banknoteM] = 0;
                    OnBanknoteChanged();
                    throw new ArgumentException("Count is greater than stock!");
                }
                else
                {
                    _banknotes[banknoteM] -= count;
                    OnBanknoteChanged();
                }
            }
            else
            {
                throw new ArgumentException("Value of banknote is incorrect!");
            }
        }

        public IDictionary<decimal, int> GetAllBanknotes()
        {
            return _banknotes;
        }
    }
}
