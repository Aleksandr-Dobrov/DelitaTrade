using DelitaTrade.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Contracts
{
    public interface IVehicleService
    {
        Task<IEnumerable<VehicleViewModel>> AllAsync();
        Task<int> CreateAsync(VehicleViewModel vehicle);
        Task UpdateAsync(VehicleViewModel vehicle);
        Task DeleteSoftAsync(int id);
    }
}
