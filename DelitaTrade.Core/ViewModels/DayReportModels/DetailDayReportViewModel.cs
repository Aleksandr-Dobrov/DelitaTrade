using DelitaTrade.Core.ViewModels.DeliveryModels;

namespace DelitaTrade.Core.ViewModels.DayReportModels
{
    public class DetailDayReportViewModel
    {
        public int Id { get; set; }
        public DateTime ReportedDate { get; set; }
        public required string EmployeeName { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal TotalCash { get; set; }

        public int DeliveriesCount { get; set; }
        public int PaymentsCount { get; set; }

        public IEnumerable<DeliveryViewModel> Deliveries { get; set; } = new List<DeliveryViewModel>();
    }
}
