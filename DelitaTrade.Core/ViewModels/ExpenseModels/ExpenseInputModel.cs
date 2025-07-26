using System.ComponentModel.DataAnnotations;
using static DelitaTrade.Common.ValidationConstants;

namespace DelitaTrade.Core.ViewModels.ExpenseModels
{
    public class ExpenseInputModel
    {
        public int DayReportId { get; set; }
        public int VehicleId { get; set; }
        [MinLength(CompanyObjectNameMinLength)]
        [MaxLength(CompanyObjectNameMaxLength)]
        public string? Expense {  get; set; }
        public int? ExpenseId { get; set; }
        [Range(0.0, 2000.0)]
        public decimal ExpenseAmount { get; set; }
        public IEnumerable<ExpenseDropDownModel> Expenses { get; set; } = new List<ExpenseDropDownModel>();
    }
}
