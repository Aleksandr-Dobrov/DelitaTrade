namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class SearchDayReportInputModel
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string? ReporterName { get; set; }
        public string? ReporterId { get; set; }

        public IEnumerable<UserViewModel> Employees { get; set; } = new List<UserViewModel>();

        public IEnumerable<SimpleDayReportViewModel> DayReports { get; set; } = new List<SimpleDayReportViewModel>();
    }
}
