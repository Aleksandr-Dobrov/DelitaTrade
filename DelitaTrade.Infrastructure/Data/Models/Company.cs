using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Common;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(ValidationConstants.CompanyNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string Name { get; set; } = null!;
        [MaxLength(ValidationConstants.CompanyTypeMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? Type { get; set; }
        [MaxLength(ValidationConstants.CompanyBulstadMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? Bulstad {  get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<CompanyObject> Objects { get; set; } = new List<CompanyObject>();
    }
}
