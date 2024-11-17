using DelitaTrade.Models.Interfaces.DataBase;
using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class PayDesk : IDBData, ICloneable
    {
        private int _id;
        Dictionary<decimal, int> _banknotes;
        private decimal _amount;

        public PayDesk()
        {
            BanknotesInitialize();            
        }

        public PayDesk(int id) : this() 
        {
            _id = id;
        }

        public decimal Amount => _amount;
        public int Id => _id;

        public string AllBankcote 
        {
            get 
            {
                string banknote = string.Empty;
                foreach (var item in _banknotes)
                {
                    banknote += $"[{item.Key} => {item.Value}]";
                }
                return banknote;
            }
        }

        public string Parameters => throw new NotImplementedException();

        public string Data => throw new NotImplementedException();

        public string Procedure => throw new NotImplementedException();

        public int NumberOfAdditionalParameters => throw new NotImplementedException();

        public object Clone()
        {
            var clone = new PayDesk(Id);
            foreach (var banknote in _banknotes)
            {
                if (banknote.Value > 0)
                { 
                    clone.AddMoney(banknote.Key.ToString(), banknote.Value);
                }
            }

            return clone;
        }

        public void OnBanknoteChanged()
        {
            CalculationAmount();
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

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            var payDesk = obj as PayDesk;
            return payDesk.Id == Id;
        }

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

        private void CalculationAmount()
        {
            _amount = _banknotes.Sum(b => b.Value * b.Key);
        }
    }
}
