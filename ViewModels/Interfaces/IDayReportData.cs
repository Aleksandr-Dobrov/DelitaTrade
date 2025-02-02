using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface IDayReportData : INotifyPropertyChanged
    {
        DateTime Date { get; }
        bool HasDayReportLoad { get; }

        int CurrentDayReportId { get; }
    }
}
