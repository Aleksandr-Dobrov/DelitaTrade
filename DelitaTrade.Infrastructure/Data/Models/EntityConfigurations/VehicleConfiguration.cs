using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static DelitaTrade.Common.DelitaDbConstants;

namespace DelitaTrade.Infrastructure.Data.Models.EntityConfigurations
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasData
            (
                new Vehicle
                {
                    Id = DefaultVehicleId,
                    LicensePlate = DefaultVehicleLicensePlate,
                    IsActive = true                    
                }
            );
        }
    }
}
