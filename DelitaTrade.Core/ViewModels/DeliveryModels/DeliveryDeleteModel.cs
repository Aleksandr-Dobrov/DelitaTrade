namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class DeliveryDeleteModel
    {
        public int Id { get; set; }
        public int DayReportId { get; set; }
        public required string EmployeeName { get; set; }
        public required string DeliveryAddress { get; set; }
        public IEnumerable<string> Payments { get; set; } = new List<string>();
    }
}
