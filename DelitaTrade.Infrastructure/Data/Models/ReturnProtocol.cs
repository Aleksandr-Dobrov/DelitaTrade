using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

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
        public Company Company { get; set; } = null!;
        [ForeignKey(nameof(CompanyObjectId))]
        public int CompanyObjectId { get; set; }
        public CompanyObject Object { get; set; } = null!;
        public int TraderId { get; set; }
        [ForeignKey(nameof(TraderId))]
        public Trader Trader { get; set; } = null!;
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ReturnedProduct> ReturnedProducts { get; set; } = new List<ReturnedProduct>();
    }
}
