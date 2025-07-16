using DelitaTrade.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DelitaTrade.Common.ValidationTypeConstants;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class InvoiceInDayReport
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = Money)]
        public decimal Income { get; set; }
        public PayMethod PayMethod { get; set; }
        public bool IsCompleted { get; set; }
        public int DayReportId { get; set; }
        [ForeignKey(nameof(DayReportId))]
        public required DayReport DayReport { get; set; }
        public int InvoiceId { get; set; }
        [ForeignKey(nameof(InvoiceId))]
        public required Invoice Invoice { get; set; }
    }
}
