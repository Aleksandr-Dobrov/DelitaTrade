using DelitaTrade.Components.ComponentsCommands;
using DelitaTrade.Core.ViewModels;
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
        private string _amount = string.Empty;
        private readonly string _imagePath;

        private DayReportViewModel? _dayReportViewModel;

        public BanknoteViewModel(IServiceProvider serviceProvider, decimal value, string imagePath)
        {  
            _value = value;
            _imagePath = imagePath;
            AddBanknote = new AddBanknotesCommand(this, serviceProvider);
            RemoveBanknote = new RemoveBanknotesCommand(this, serviceProvider);
            TotalCountChanged += CalculateAmount;
            BanknoteChange += () => { };
        }

        public event Action TotalCountChanged;

        public event Action BanknoteChange;

        public int DayReportId => _dayReportViewModel != null ? _dayReportViewModel.Id : 0;

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

        public void OnDayReportSelected(DayReportViewModel dayReportViewModel)
        {
            _dayReportViewModel = dayReportViewModel;
            Count = 1;
        }

        public void OnBanknoteChange()
        {
            BanknoteChange();
        }

        public void OnDayReportUnselected()
        {
            _dayReportViewModel = null;
            Amount = 0.ToString("C");
            Count = 1;
            TotalCount = 0;
        }

        private void CalculateAmount()
        {
            Amount = (TotalCount * Value).ToString("C");
        }
    }
}
