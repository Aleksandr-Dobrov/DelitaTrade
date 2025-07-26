using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Core.ViewModels;
using static DelitaTrade.Common.ExceptionMessages;
using System.IO;
using DelitaTrade.ViewModels.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using DelitaTrade.Services;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.Loggers;

namespace DelitaTrade.ViewModels
{
    public class PayDeskViewModel : ViewModelBase
    {
        private DayReportViewModel? _currentDayReportViewModel;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<decimal, BanknoteViewModel> _banknoteViewModels;
        private readonly DelitaSoundService _soundService;

        private const string _greenColor = "#2cff3e";
        private const string _yellowColor = "#edf239";
        private const string _redColor = "#ff2c45";

        private static string iconPath = string.Empty;
        private string _income;
        private string _amount;
        private string _neededAmount;
        private string _neededColor = "Transparent";
        private bool _isEditable;
        private decimal _totalAmount;

        public PayDeskViewModel(DelitaSoundService delitaSound, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _soundService = delitaSound;
            GetIconFilePath();
            _banknoteViewModels = new Dictionary<decimal, BanknoteViewModel>();
            BanknoteChange += PlayMoneyChangeSound;
            BanknoteChange += OnBanknotesChange;
            _banknoteViewModels[0.01m] = new BanknoteViewModel(_serviceProvider,
               0.01m, $"{iconPath}0.01lv.png");
            _banknoteViewModels[0.02m] = new BanknoteViewModel(_serviceProvider,
                0.02m, $"{iconPath}0.02lv.png");
            _banknoteViewModels[0.05m] = new BanknoteViewModel(_serviceProvider,
                0.05m, $"{iconPath}0.05lv.png");
            _banknoteViewModels[0.1m] = new BanknoteViewModel(_serviceProvider,
                0.1m, $"{iconPath}0.1lv.png");
            _banknoteViewModels[0.2m] = new BanknoteViewModel(_serviceProvider,
                0.2m, $"{iconPath}0.2lv.png");
            _banknoteViewModels[0.5m] = new BanknoteViewModel(_serviceProvider,
                0.5m, $"{iconPath}0.5lv.png");
            _banknoteViewModels[1m] = new BanknoteViewModel(_serviceProvider,
                1m, $"{iconPath}1lv.png");
            _banknoteViewModels[2m] = new BanknoteViewModel(_serviceProvider,
                2m, $"{iconPath}2lv.png");
            _banknoteViewModels[5m] = new BanknoteViewModel(_serviceProvider,
                5m, $"{iconPath}5lv.png");
            _banknoteViewModels[10m] = new BanknoteViewModel(_serviceProvider,
                10m, $"{iconPath}10lv.png");
            _banknoteViewModels[20m] = new BanknoteViewModel(_serviceProvider,
                20m, $"{iconPath}20lv.png");
            _banknoteViewModels[50m] = new BanknoteViewModel(_serviceProvider,
                50m, $"{iconPath}50lv.png");
            _banknoteViewModels[100m] = new BanknoteViewModel(_serviceProvider,
                100m, $"{iconPath}100lv.png");
            foreach (var banknote in _banknoteViewModels.Values)
            {
                banknote.BanknoteChange += BanknoteChange;
            }
        }

        public Dictionary<decimal, BanknoteViewModel> BanknoteViewModel => _banknoteViewModels;

        public event Action BanknoteChange;


        public bool IsEditable
        {
            get => _isEditable;
            set 
            {
                _isEditable = value;
                OnPropertyChange();
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

        public void OnDayReportSelected(DayReportViewModel dayReportViewModel)
        {
            foreach (var banknote in _banknoteViewModels.Values)
            {
                banknote.OnDayReportSelected(dayReportViewModel);
            }
            _currentDayReportViewModel = dayReportViewModel;
            IsEditable = true;
        }

        public void OnDayReportUnselected()
        {
            foreach (var banknote in _banknoteViewModels.Values)
            {
                banknote.OnDayReportUnselected();
            }
            IsEditable = false;
            _totalAmount = 0;
            Amount = 0.ToString("C");
            Income = 0.ToString("C");
            NeededAmount = 0.ToString("C");
            NeededColor = _redColor;
            _currentDayReportViewModel = null;
        }

        public void GetAllBanknotes()
        {
            if (_currentDayReportViewModel == null) throw new ArgumentNullException(NotFound(nameof(DayReportViewModel)));

            foreach (var model in _currentDayReportViewModel.Banknotes)
            {
                _banknoteViewModels[model.Key].TotalCount = model.Value;
            }
            CalculateAmount();
            OnDayReportTotalsChanged();
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
            _totalAmount = amount;
            Amount = amount.ToString("C");
            NeededAmount = (_currentDayReportViewModel!.TotalIncome - amount).ToString("C");//total income from day report
            SetNeededColor(amount, _currentDayReportViewModel.TotalIncome);
        }

        private void OnDayReportTotalsChanged()
        {
            Income = _currentDayReportViewModel!.TotalIncome.ToString("C");
        }

        private void SetNeededColor(decimal amount, decimal totalIncome)
        {
            switch (totalIncome - amount)
            {
                case > 0:
                    NeededColor = _redColor;
                    break;
                case 0:
                    NeededColor = _greenColor;
                    break;
                case < 0:
                    NeededColor = _yellowColor;
                    break;
            }
        }

        private async void OnBanknotesChange()
        {
            if (_currentDayReportViewModel == null) throw new ArgumentNullException(NotFound(nameof(DayReportViewModel)));

            IDayReportCrudController banknotesService = _serviceProvider.GetRequiredService<IDayReportCrudController>();
            var dayReport = await banknotesService.ReadDayReportBanknotesByIdAsync(_currentDayReportViewModel.Id);
            _currentDayReportViewModel.Banknotes = dayReport.Banknotes;
            GetAllBanknotes();
            _currentDayReportViewModel.TotalCash = _totalAmount;
            await banknotesService.UpdateDayReportAsync(_currentDayReportViewModel);
        }
        private void PlayMoneyChangeSound()
        {
            try
            {
                _soundService.PlaySound(SoundEfect.Cash);
            }
            catch (ArgumentException ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error);
            }
        }
    }
}
