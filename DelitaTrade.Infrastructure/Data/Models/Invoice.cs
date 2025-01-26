using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DelitaTrade.Common.ValidationConstants;
using static DelitaTrade.Common.ValidationTypeConstants;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(InvoiceNumberMaxLength)]
        public required string Number { get; set; }
        [Column(TypeName = Money)]
        public decimal Amount { get; set; }
        public decimal Weight { get; set; }
        public bool IsPaid { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required Company Company { get; set; }
        public int CompanyObjectId { get; set; }
        [ForeignKey(nameof(CompanyObjectId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required CompanyObject CompanyObject { get; set; }
        public ICollection<InvoiceInDayReport> InvoicesInDayReports { get; set; } = new List<InvoiceInDayReport>();
    }
}
