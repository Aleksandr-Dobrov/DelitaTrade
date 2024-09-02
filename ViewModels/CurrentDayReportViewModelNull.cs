using DelitaTrade.Models;

namespace DelitaTrade.ViewModels
{
    class CurrentDayReportViewModelNull : CurrentDayReportViewModel
    {
        private string _vehicle = "Empty";

        public CurrentDayReportViewModelNull() : base(new DayReport("Not Load"))
        {            
        }

        public string Vehicle => _vehicle;
    }
}
