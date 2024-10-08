using DelitaTrade.Components.ComponentsCommands;
using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class BanknoteViewModel : ViewModelBase
    {
        private readonly decimal _value;
        private int _count = 1;
        private int _totalCount;
        private string _amount;
        private readonly string _imagePath;

        public BanknoteViewModel(DelitaTradeDayReport delitaTradeDayReport, decimal value, string imagePath)
        {  
            _value = value;
            _imagePath = imagePath;
            AddBanknote = new AddBanknotesCommand(this, delitaTradeDayReport);
            RemoveBanknote = new RemoveBanknotesCommand(this, delitaTradeDayReport);
            TotalCountChanged += CalculateAmount;            
        }

        public event Action TotalCountChanged;

        public string ImagePath => _imagePath;
        public decimal Value => _value;

        public int TotalCount
        {
            get => _totalCount;
            set 
            {
                _totalCount = value;
                TotalCountChanged();
                OnPropertyChange(nameof(TotalCount));
            }
        }

        public string Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChange();
            }                
        }

        public int Count
        {
            get => _count;
            set 
            {
                _count = value;
                OnPropertyChange();
            }
        }

        public ICommand AddBanknote { get; }
        public ICommand RemoveBanknote { get; }

        private void CalculateAmount()
        {
            Amount = (TotalCount * Value).ToString("C");
        }
    }
}
