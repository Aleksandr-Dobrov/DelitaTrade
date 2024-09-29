namespace DelitaTrade.Models.Interfaces.DayReports
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
