namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class SimpleDayReportViewModel
    {
        public int Id { get; set; }
        public DateTime ReportedDate { get; set; }
        public required string ReporterName { get; set; }
        public required string TotalAmount { get; set; }
        public required string TotalIncome { get; set; }
        public required string TotalCash { get; set; }
        public string? VehicleLicensePlate { get; set; } = string.Empty;
        public DateTime? TransmissionDate { get; set; }
    }
}
