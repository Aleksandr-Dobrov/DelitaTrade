using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DelitaTrade.Common;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class Trader
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.TraderNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Name { get; set; } = null!;        
        [MaxLength(ValidationConstants.TraderPhoneNumberMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<CompanyObject> Objects { get; set; } = new HashSet<CompanyObject>();
    }
}
