using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DelitaTrade.Common.ValidationTypeConstants;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class DayReport
    {
        [Key]
        public int Id { get; set; }
        public required DateTime Date { get; set; }
        [Column(TypeName = Money)]
        public decimal TotalAmount { get; set; }
        [Column(TypeName = Money)]
        public decimal TotalIncome { get; set; }
        [Column(TypeName = Money)]
        public decimal TotalExpense { get; set; }
        [Column(TypeName = Money)]
        public decimal TotalNotPay { get; set; }
        [Column(TypeName = Money)]
        public decimal TotalOldInvoice { get; set; }
        public decimal TotalWeight { get; set; }
        public required Dictionary<decimal, int> Banknotes { get; set; } = new();
        [Column(TypeName = Money)]
        public decimal TotalCash { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public required User User { get; set; }
        public int? VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public Vehicle? Vehicle { get; set; }
        public ICollection<InvoiceInDayReport> Invoices { get; set; } = new List<InvoiceInDayReport>();
    }
}
