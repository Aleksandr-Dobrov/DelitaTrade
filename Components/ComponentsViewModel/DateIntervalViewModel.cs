using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class DateIntervalViewModel : ValidationViewModel
    {
        private const string DarkRedColor = "#cc0000";
        private const string DarkGreenColor = "#009900";

        private DateTime _startDate = DateTime.Now;
        private DateTime _endDate = DateTime.Now;

        public DateIntervalViewModel()
        {
            DateIntervalChanged += () => { };
        }

        public event Action DateIntervalChanged;

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChange(nameof(EndDateColor));
                    OnPropertyChange(nameof(StartDate));
                    DateIntervalChanged();
                }
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChange(nameof(EndDateColor));
                    OnPropertyChange(nameof(EndDate));
                    DateIntervalChanged();
                }
            }
        }

        public string EndDateColor => StartDate > EndDate ? DarkRedColor : DarkGreenColor;
    }
}
