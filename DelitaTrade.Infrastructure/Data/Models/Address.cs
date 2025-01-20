using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DelitaTrade.Common;

namespace DelitaTrade.Infrastructure.Data.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(ValidationConstants.TownNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public required string Town { get; set; }

        [MaxLength(ValidationConstants.StreetNameMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? StreetName { get; set; }

        [MaxLength(ValidationConstants.StreetNumberMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? Number { get; set; }

        [MaxLength(ValidationConstants.GpsCoordinatesMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? GpsCoordinates { get; set; }

        [MaxLength(ValidationConstants.AddressDescriptionMaxLength)]
        [Column(TypeName = ValidationTypeConstants.NVarchar)]
        public string? Description { get; set; }

        public ICollection<CompanyObject> CompanyObjects { get; set; } = new List<CompanyObject>();
    }
}
