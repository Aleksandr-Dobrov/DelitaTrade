using System.ComponentModel.DataAnnotations;
using static DelitaTrade.Common.ValidationConstants;
namespace DelitaTrade.Infrastructure.Data.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(LicensePlateMaxLength)]
        public required string LicensePlate { get; set; }
        [MaxLength(VehicleModelMaxLength)]
        public string? Model { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
