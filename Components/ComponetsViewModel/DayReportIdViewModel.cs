using System.Collections.ObjectModel;
using DelitaTrade.Models;
using DelitaTrade.Interfaces;
using DelitaTrade.Interfaces.DayReport;
using System.ComponentModel;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponetsViewModel
{
    public class DayReportIdViewModel : ViewModelBase
    {
        private Dictionary<string, Dictionary<string, ObservableCollection<string>>> _yearMonthDay;

        private IDayReportIdDataBese _dayReportIdDataBase;

        private HashSet<string> _dayReportIds;

        private string _day;
        private string _month;
        private string _year;

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

        public string DayReportId => $"{Year}-{Month}-{Day}";

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

        public DayReportIdViewModel(IDayReportIdDataBese dayReportIdDataBase)
        {
            _dayReportIdDataBase = dayReportIdDataBase;
            _yearMonthDay = new Dictionary<string, Dictionary<string, ObservableCollection<string>>>();
            _dayReportIds = new HashSet<string>();
            LoadDayReportIds();
            _dayReportIdDataBase.DayReportIdsLoad += YearMonthDayUpdate;
            YearMonthDayUpdate(_dayReportIdDataBase.DayReportsId.ToList());
            DayReportIdExists += () => { };
            DataBaseChange += () => { };
            PropertyChanged += OnDateChange;
            _dayReportIdDataBase.DayReportIdAdd += YearMonthDayAdd;
            _dayReportIdDataBase.DayReportIdRemove += YearMontDayRemove;
            _dayReportIdDataBase.DayReportsIdChanged += OnDataBaseChange;
        }

        private void YearMonthDayUpdate(List<string> dayReportIds)
        {
            _yearMonthDay.Clear();
            foreach (var day in dayReportIds)
            {
                AddDayToYearMontDay(day);
            }
            UpdateDateProperties();
        }

        private void YearMonthDayAdd(string dayReportId)
        {
            _dayReportIds.Add(dayReportId);
            AddDayToYearMontDay(dayReportId);
            UpdateDateProperties();
        }

        private void YearMontDayRemove(string dayReportId)
        {
            _dayReportIds.Remove(dayReportId);
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

        private void LoadDayReportIds()
        {
            foreach (var Id in _dayReportIdDataBase.DayReportsId)
            {
                _dayReportIds.Add(Id);
            }
        }

        private bool IsExistsDayReportIdInDataBase()
        {
            return _dayReportIds.Contains(DayReportId);
        }

        private void CheckDayReportId()
        {
            if (IsExistsDayReportIdInDataBase())
            {
                DayReportIdExists.Invoke();
            }
        }

        private void OnDateChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Year)
                || e.PropertyName == nameof(Month)
                || e.PropertyName == nameof(Day))
            {
                CheckDayReportId();
            }
        }

        private void OnDataBaseChange()
        {
            DataBaseChange.Invoke();
        }

        public event Action DayReportIdExists;

        public event Action DataBaseChange;
    }
}
