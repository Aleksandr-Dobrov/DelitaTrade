using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models;
using System.IO;

namespace DelitaTrade.ViewModels
{
    public class PayDeskViewModel : ViewModelBase
    {
        private readonly DelitaTradeDayReport _dayReportCreator;

        private readonly Dictionary<decimal, BanknoteViewModel> _banknoteViewModels;

        private static string iconPath = string.Empty;

        private string _income;
        private string _amount;
        private string _neededAmount;
        private string _neededColor = "Transparent";

        public Dictionary<decimal, BanknoteViewModel> BanknoteViewModel => _banknoteViewModels;

        public PayDeskViewModel(DelitaTradeDayReport delitaTradeDayReport)
        {
            GetIconFilePath();
            _banknoteViewModels = new Dictionary<decimal, BanknoteViewModel>();
            _dayReportCreator = delitaTradeDayReport;
            _banknoteViewModels[0.01m] = new BanknoteViewModel(delitaTradeDayReport,
               0.01m, $"{iconPath}0.01lv.png");
            _banknoteViewModels[0.02m] = new BanknoteViewModel(delitaTradeDayReport,
                0.02m, $"{iconPath}0.02lv.png");
            _banknoteViewModels[0.05m] = new BanknoteViewModel(delitaTradeDayReport,
                0.05m, $"{iconPath}0.05lv.png");
            _banknoteViewModels[0.1m] = new BanknoteViewModel(delitaTradeDayReport,
                0.1m, $"{iconPath}0.1lv.png");
            _banknoteViewModels[0.2m] = new BanknoteViewModel(delitaTradeDayReport,
                0.2m, $"{iconPath}0.2lv.png");
            _banknoteViewModels[0.5m] = new BanknoteViewModel(delitaTradeDayReport,
                0.5m, $"{iconPath}0.5lv.png");
            _banknoteViewModels[1m] = new BanknoteViewModel(delitaTradeDayReport,
                1m, $"{iconPath}1lv.png");
            _banknoteViewModels[2m] = new BanknoteViewModel(delitaTradeDayReport,
                2m, $"{iconPath}2lv.png");
            _banknoteViewModels[5m] = new BanknoteViewModel(delitaTradeDayReport,
                5m, $"{iconPath}5lv.png");
            _banknoteViewModels[10m] = new BanknoteViewModel(delitaTradeDayReport,
                10m, $"{iconPath}10lv.png");
            _banknoteViewModels[20m] = new BanknoteViewModel(delitaTradeDayReport,
                20m, $"{iconPath}20lv.png");
            _banknoteViewModels[50m] = new BanknoteViewModel(delitaTradeDayReport,
                50m, $"{iconPath}50lv.png");
            _banknoteViewModels[100m] = new BanknoteViewModel(delitaTradeDayReport,
                100m, $"{iconPath}100lv.png");
            delitaTradeDayReport.DayReportDataChanged += GetAllBanknotes;
            delitaTradeDayReport.TotalsChanged += OnDayReportTotalsChanged;                     
        }

        private void GetIconFilePath()
        {
            FileInfo fileInfo = new("../../../Components/ComponentAssets/Banknotes/0.01lv.jpg");
            int index = fileInfo.FullName.LastIndexOf('\\') + 1;
            iconPath = fileInfo.FullName[..index];
        }

        private void CalculateAmount()
        {
            decimal amount = 0;

            foreach (var item in _banknoteViewModels)
            {
                amount += item.Key * item.Value.TotalCount;
            }
            Amount = amount.ToString("C");
            NeededAmount = (_dayReportCreator.TotalIncome - amount).ToString("C");
            SetNeededColor(amount,_dayReportCreator.TotalIncome);
        }

        private void OnDayReportTotalsChanged()
        {
            Income = _dayReportCreator.TotalIncome.ToString("C");
        }

        private void SetNeededColor(decimal amount, decimal totalIncome)
        {
            switch (totalIncome - amount)
            {
                case > 0:
                    NeededColor = "#ff2c45";
                    break;
                case 0:
                    NeededColor = "#2cff3e";
                    break;
                case < 0:
                    NeededColor = "#edf239";
                    break;
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




        public string Income
        {
            get => _income;
            set 
            {
                _income = value;
                OnPropertyChange();
            }
        }

        public string NeededAmount
        {
            get => _neededAmount;
            set
            {
                _neededAmount = value;
                OnPropertyChange();
            }
        }
        
        public string NeededColor
        {
            get => _neededColor;
            set 
            {
                _neededColor = value;
                OnPropertyChange();
            }
        }

        public void GetAllBanknotes()
        {
            foreach (var model in _dayReportCreator.GetAllBanknotes())
            {
                _banknoteViewModels[model.Key].TotalCount = model.Value;
            }
            CalculateAmount();
        }

    }
}
