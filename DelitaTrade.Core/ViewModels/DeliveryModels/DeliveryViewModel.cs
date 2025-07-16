namespace DelitaTrade.Core.ViewModels.DeliveryModels
{
    public class DeliveryViewModel
    {
        public int Id { get; set; }
        public int dayReportId { get; set; }
        public int CompanyObjectId { get; set; }
        public required string CompanyObjectName { get; set; }
        public required string EmployeeName { get; set; }
        public bool IsBank { get; set; }

        public string? Address { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal TotalIncome { get; set; }

        public decimal TotalCash { get; set; }
        public decimal TotalBank { get; set; }
        public decimal TotalCard { get; set; }
        public decimal TotalOld {  get; set; }
        public decimal TotalWeight { get; set; }

        public IEnumerable<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
    }
}
