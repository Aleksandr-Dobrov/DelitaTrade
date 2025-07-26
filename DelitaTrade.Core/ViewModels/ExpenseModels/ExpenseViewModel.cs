using DelitaTrade.Core.ViewModels.DeliveryModels;

namespace DelitaTrade.Core.ViewModels.ExpenseModels
{
    public class ExpenseViewModel
    {
        public int DayReportId { get; set; }

        public required string ExpenseDate { get; set; }
        public decimal TotalExpense { get; set; }

        public IEnumerable<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
    }
}
