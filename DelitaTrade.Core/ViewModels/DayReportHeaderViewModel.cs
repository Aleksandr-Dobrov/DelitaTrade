using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.ViewModels
{
    public class DayReportHeaderViewModel
    {
        public int Id { get; set; }
        public required DateTime Date { get; set; }
    }
}
