using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }

        public int DeliveryAddressId { get; set; }
        [ForeignKey(nameof(DeliveryAddressId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual required CompanyObject DeliveryAddress { get; set; }
        
        public int DayReportId { get; set; }
        [ForeignKey(nameof(DayReportId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual required DayReport DayReport { get; set; }

        public int? VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual Vehicle? Vehicle { get; set; }

        public Guid EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        [DeleteBehavior(DeleteBehavior.Restrict)]
        public virtual required DelitaUser Employee { get; set; }

        public ICollection<InvoiceInDayReport> Payments { get; set; } = new HashSet<InvoiceInDayReport>();
    }
}
