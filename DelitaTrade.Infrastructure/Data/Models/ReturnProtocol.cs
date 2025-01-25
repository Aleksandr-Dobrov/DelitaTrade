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
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ReturnedProduct> ReturnedProducts { get; set; } = new List<ReturnedProduct>();
    }
}
