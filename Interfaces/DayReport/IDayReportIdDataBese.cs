using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Interfaces.DayReport
{
    public interface IDayReportIdDataBese
    {
        public IEnumerable<string> DayReportsId { get; }

        public event Action DayReportsIdChanged;

        public event Action<List<string>> DayReportIdsLoad;
        public event Action<string> DayReportIdAdd;
        public event Action<string> DayReportIdRemove;
    }
}
