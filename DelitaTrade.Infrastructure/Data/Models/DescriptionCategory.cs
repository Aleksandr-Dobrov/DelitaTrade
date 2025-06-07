using DelitaTrade.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class DescriptionCategory
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.DescriptionCategoryMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public required string Name { get; set; }
    }
}
