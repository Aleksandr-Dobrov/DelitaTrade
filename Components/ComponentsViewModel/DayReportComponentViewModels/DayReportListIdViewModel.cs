using DelitaTrade.Common;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Interfaces;
using DelitaTrade.WpfViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportListIdViewModel : ViewModelBase
    {
        private bool _isInitialized;
        private bool _isOnCollectionChange;
        private Dictionary<string, Dictionary<string, ObservableCollection<DayReportDayIdViewModel>>> _yearMonthDay;

        private HashSet<WpfDayReportIdViewModel> _dayReportIds;

        private DayReportDayIdViewModel? _day;
        private string _month = string.Empty;
        private string _year = string.Empty;

        public DayReportListIdViewModel()
        {
            _yearMonthDay = new Dictionary<string, Dictionary<string, ObservableCollection<DayReportDayIdViewModel>>>();
            _dayReportIds = new HashSet<WpfDayReportIdViewModel>();
            DayReportIdSelected += (Id) => { };
            DataBaseChange += () => { };
            PropertyChanged += OnDateChange;
        }

        public event Action<int> DayReportIdSelected;

        public event Action DataBaseChange;

        public string DayReportId => $"{Year}-{Month}-{Day?.Day}.{Day?.Id}";

        public bool IsInitialized => _isInitialized;

        public string Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChange();                
                OnPropertyChange(nameof(Months));
                OnPropertyChange(nameof(Days));
            }
        }

        public string Month
        {
            get => _month;
            set
            {
                _month = value;
                OnPropertyChange();
                OnPropertyChange(nameof(Days));
            }
        }
        public DayReportDayIdViewModel? Day
        {
            get => _day;
            set
            {
                _day = value;
                OnPropertyChange();
            }
        }

        public IEnumerable<string> Years
        {
            get
            {
                IEnumerable<string> year;
                if (_yearMonthDay.Count > 0)
                {
                    year = _yearMonthDay.Keys.Order();
                }
                else
                {
                    year = new List<string>();
                }

                return year;
            }
        }

        public IEnumerable<string> Months
        {
            get
            {
                IEnumerable<string> months;
                if (string.IsNullOrEmpty(Year) == false && _yearMonthDay[Year].Count > 0)
                {
                    months = _yearMonthDay[Year].Keys.Order();
                }
                else
                {
                    months = new List<string>();
                }

                return months;
            }
        }

        public IEnumerable<DayReportDayIdViewModel> Days
        {
            get
            {
                IEnumerable<DayReportDayIdViewModel> days;
                if (string.IsNullOrEmpty(Year) == false && string.IsNullOrEmpty(Month) == false && _yearMonthDay[Year][Month].Count > 0)
                {
                    days = _yearMonthDay[Year][Month].OrderBy(d => d.Day);
                }
                else
                {
                    days = new List<DayReportDayIdViewModel>();
                }

                return days;
            }
        }

        public async void Initialized(IDayReportCrudController dayReportController)
        {
            if (IsInitialized) return;
            dayReportController.OnCreated += YearMonthDayAdd;
            dayReportController.OnDeleted += YearMontDayRemove;
            LoadDayReportIds(await dayReportController.ReadAllHeaders());
            _isInitialized = true;
        }

        public async void UpdateCollection(IDayReportCrudController dayReportController)
        {
            if (IsInitialized == false) throw new InvalidOperationException(ExceptionMessages.NotInitialized(nameof(DayReportListIdViewModel)));
            LoadDayReportIds(await dayReportController.ReadAllHeaders());
        }

        private void YearMonthDayUpdate()
        {
            _yearMonthDay.Clear();
            foreach (var day in _dayReportIds)
            {
                AddDayToYearMontDay(day);
            }
            UpdateDateProperties();
        }

        private void YearMonthDayAdd(DayReportViewModel dayReport)
        {
            _isOnCollectionChange = true;
            WpfDayReportIdViewModel newDayReportId = new () { Id = dayReport.Id, Date = dayReport.Date };
            _dayReportIds.Add(newDayReportId);
            var day = AddDayToYearMontDay(newDayReportId);
            UpdateDateProperties();
            Year = $"{dayReport.Date:yyyy}";
            Month = $"{dayReport.Date:MM}";
            Day = day;
            _isOnCollectionChange = false;
        }

        private void YearMontDayRemove(WpfDayReportIdViewModel dayReport)
        {
            var date = _dayReportIds.FirstOrDefault(d => d.Id == dayReport.Id) ?? throw new InvalidOperationException("Incorrect Id");
            string year = $"{date.Date:yyyy}";
            string month = $"{date.Date:MM}";
            DayReportDayIdViewModel day = new () { Id = dayReport.Id, Day = $"{date.Date:dd}" };
            _dayReportIds.Remove(dayReport);

            _yearMonthDay[year][month].Remove(day);
            if (_yearMonthDay[year][month].Count == 0)
            {
                _yearMonthDay[year].Remove(month);
            }
            if (_yearMonthDay[year].Count == 0)
            {
                _yearMonthDay.Remove(year);
            }
            UpdateDateProperties();

        }

        private DayReportDayIdViewModel AddDayToYearMontDay(WpfDayReportIdViewModel dayReport)
        {
            string year = $"{dayReport.Date:yyyy}";
            string month = $"{dayReport.Date:MM}";
            DayReportDayIdViewModel day = new() { Id = dayReport.Id, Day = $"{dayReport.Date:dd}" };

            if (_yearMonthDay.ContainsKey(year) == false)
            {
                _yearMonthDay[year] = new Dictionary<string, ObservableCollection<DayReportDayIdViewModel>>();
                Year = year;
            }
            if (_yearMonthDay[year].ContainsKey(month) == false)
            {
                _yearMonthDay[year][month] = new ObservableCollection<DayReportDayIdViewModel>();
                Month = month;
            }
            _yearMonthDay[year][month].Add(day);
            UpdateDateProperties();
            return day;
        }

        private void UpdateDateProperties()
        {
            OnPropertyChange(nameof(Years));
            OnPropertyChange(nameof(Months));
            OnPropertyChange(nameof(Days));
        }

        private void LoadDayReportIds(IEnumerable<DayReportHeaderViewModel> dayReports)
        {
            foreach (var dayReport in dayReports)
            {
                _dayReportIds.Add(new WpfDayReportIdViewModel() { Id = dayReport.Id, Date = dayReport.Date});
            }
            YearMonthDayUpdate();
        }

        private bool IsExistsDayReportIdInDataBase()
        {
            if (DayReportId.Contains('.') == false) return false;
            if (DateTime.TryParse(DayReportId.Split('.')[0], out DateTime date) == false) return false;

            return _dayReportIds.Contains(new WpfDayReportIdViewModel() { Id = int.Parse(DayReportId.Split('.')[1]), Date = date });
        }

        private void CheckDayReportId()
        {
            if (IsExistsDayReportIdInDataBase())
            {
                DayReportIdSelected.Invoke(int.Parse(DayReportId.Split('.')[1]));
            }
        }

        private void OnDateChange(object? sender, PropertyChangedEventArgs e)
        {
            if (_isOnCollectionChange == false && (e.PropertyName == nameof(Year)
                || e.PropertyName == nameof(Month)
                || e.PropertyName == nameof(Day)))
            {
                CheckDayReportId();
            }
        }
    }
}
