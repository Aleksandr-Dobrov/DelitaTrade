using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;
using Microsoft.EntityFrameworkCore;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class ReturnProtocol
    {
        [Key]
        public int Id { get; set; }
        public DateTime ReturnedDate { get; set; }
        public DateTime? LastChanged { get; set; }
        [MaxLength(ValidationConstants.ReturnProtocolPayMethodLength)]
        public string PayMethod { get; set; } = null!;
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Company Company { get; set; } = null!;
        public int CompanyObjectId { get; set; }
        [ForeignKey(nameof(CompanyObjectId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public CompanyObject Object { get; set; } = null!;
        public int TraderId { get; set; }
        [ForeignKey(nameof(TraderId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public Trader Trader { get; set; } = null!;
        
        public Guid IdentityUserId { get; set; }

        [ForeignKey(nameof(IdentityUserId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public required virtual DelitaUser IdentityUser { get; set; }

        public Guid? ApproverId { get; set; }

        [ForeignKey(nameof(ApproverId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual DelitaUser? Approver { get; set; }
        public virtual ICollection<ReturnedProduct> ReturnedProducts { get; set; } = new List<ReturnedProduct>();
    }
}
