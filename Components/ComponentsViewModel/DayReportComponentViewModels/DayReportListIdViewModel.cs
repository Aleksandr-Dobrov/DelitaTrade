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
        private Dictionary<string, Dictionary<string, ObservableCollection<string>>> _yearMonthDay;

        private HashSet<WpfDayReportIdViewModel> _dayReportIds;

        private string _day = string.Empty;
        private string _month = string.Empty;
        private string _year = string.Empty;

        public DayReportListIdViewModel()
        {
            _yearMonthDay = new Dictionary<string, Dictionary<string, ObservableCollection<string>>>();
            _dayReportIds = new HashSet<WpfDayReportIdViewModel>();
            DayReportIdSelected += (Id) => { };
            DataBaseChange += () => { };
            PropertyChanged += OnDateChange;
        }

        public event Action<int> DayReportIdSelected;

        public event Action DataBaseChange;

        public string DayReportId => $"{Year}-{Month}-{Day}";

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
        public string Day
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

        public IEnumerable<string> Days
        {
            get
            {
                IEnumerable<string> days;
                if (string.IsNullOrEmpty(Year) == false && string.IsNullOrEmpty(Month) == false && _yearMonthDay[Year][Month].Count > 0)
                {
                    days = _yearMonthDay[Year][Month].Order();
                }
                else
                {
                    days = new List<string>();
                }

                return days;
            }
        }

        public async void Initialized(IDayReportCrudController dayReportController)
        {
            if (IsInitialized) return;
            dayReportController.OnCreated += YearMonthDayAdd;
            dayReportController.OnDeleted += YearMontDayRemove;
            LoadDayReportIds(await dayReportController.ReadAll());
            _isInitialized = true;
        }

        public async void UpdateCollection(IDayReportCrudController dayReportController)
        {
            if (IsInitialized == false) throw new InvalidOperationException(ExceptionMessages.NotInitialized(nameof(DayReportListIdViewModel)));
            LoadDayReportIds(await dayReportController.ReadAll());
        }

        private void YearMonthDayUpdate()
        {
            _yearMonthDay.Clear();
            foreach (var day in _dayReportIds)
            {
                AddDayToYearMontDay($"{day.Date:yyyy-MM-dd}.{day.Id}");
            }
            UpdateDateProperties();
        }

        private void YearMonthDayAdd(DayReportViewModel dayReport)
        {
            _isOnCollectionChange = true;
            _dayReportIds.Add(new WpfDayReportIdViewModel() { Id = dayReport.Id, Date = dayReport.Date });
            AddDayToYearMontDay($"{dayReport.Date:yyyy-MM-dd}.{dayReport.Id}");
            UpdateDateProperties();
            Year = $"{dayReport.Date:yyyy}";
            Month = $"{dayReport.Date:MM}";
            Day = $"{dayReport.Date:dd}.{dayReport.Id}";
            _isOnCollectionChange = false;
        }

        private void YearMontDayRemove(WpfDayReportIdViewModel dayReport)
        {
            _dayReportIds.Remove(dayReport);
            string[] yearMonthDay = DayReportId.Split('-');
            _yearMonthDay[yearMonthDay[0]][yearMonthDay[1]].Remove(yearMonthDay[2]);
            if (_yearMonthDay[yearMonthDay[0]][yearMonthDay[1]].Count == 0)
            {
                _yearMonthDay[yearMonthDay[0]].Remove(yearMonthDay[1]);
            }
            if (_yearMonthDay[yearMonthDay[0]].Count == 0)
            {
                _yearMonthDay.Remove(yearMonthDay[0]);
            }
            UpdateDateProperties();

        }

        private void AddDayToYearMontDay(string DayReportId)
        {
            string[] yearMonthDay = DayReportId.Split('-');
            if (_yearMonthDay.ContainsKey(yearMonthDay[0]) == false)
            {
                _yearMonthDay[yearMonthDay[0]] = new Dictionary<string, ObservableCollection<string>>();
                Year = yearMonthDay[0];
            }
            if (_yearMonthDay[yearMonthDay[0]].ContainsKey(yearMonthDay[1]) == false)
            {
                _yearMonthDay[yearMonthDay[0]][yearMonthDay[1]] = new ObservableCollection<string>();
                Month = yearMonthDay[1];
            }
            _yearMonthDay[yearMonthDay[0]][yearMonthDay[1]].Add(yearMonthDay[2]);
            UpdateDateProperties();
        }

        private void UpdateDateProperties()
        {
            OnPropertyChange(nameof(Years));
            OnPropertyChange(nameof(Months));
            OnPropertyChange(nameof(Days));
        }

        private void LoadDayReportIds(IEnumerable<DayReportViewModel> dayReports)
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
