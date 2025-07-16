namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class DayReportDeleteModel
    {
        public int Id { get; set; }
        public DateTime ReportedDate { get; set; }
        public required string EmployeeName { get; set; }
    }
}
