using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Common;
using DelitaTrade.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Core.Services
{
    public class VehicleService(IRepository repo) : IVehicleService
    {
        public async Task<IEnumerable<VehicleViewModel>> AllAsync()
        {
            return await repo.AllReadonly<Vehicle>()
                .Select(v => new VehicleViewModel() 
                { 
                    Id = v.Id,
                    LicensePlate = v.LicensePlate,
                    Model = v.Model                    
                }
                ).ToArrayAsync();
        }

        public async Task<VehicleViewModel> CreateAsync(VehicleViewModel vehicle)
        {
            if (await repo.GetByIdAsync<Vehicle>(vehicle.Id) != null) throw new ArgumentException(IsExists(vehicle));
            var newVehicle = new Vehicle() 
            {
                LicensePlate = vehicle.LicensePlate, 
                Model = vehicle.Model 
            };
            await repo.AddAsync(newVehicle);
            await repo.SaveChangesAsync();
            await repo.ReloadAsync(newVehicle);
            vehicle.Id = newVehicle.Id;
            return vehicle;
        }

        public async Task DeleteSoftAsync(int id)
        {
            var vehicle = await repo.GetByIdAsync<Vehicle>(id) ?? throw new ArgumentNullException(NotFound(nameof(Vehicle)));
            vehicle.IsActive = false;
            await repo.SaveChangesAsync();
        }

        public async Task UpdateAsync(VehicleViewModel vehicle)
        {
            var vehicleToUpdate = await repo.GetByIdAsync<Vehicle>(vehicle.Id) ?? throw new ArgumentNullException(NotFound(nameof(Vehicle)));
            vehicleToUpdate.Model = vehicle.Model;
            vehicleToUpdate.LicensePlate = vehicle.LicensePlate;
            await repo.SaveChangesAsync();
        }
    }
}
