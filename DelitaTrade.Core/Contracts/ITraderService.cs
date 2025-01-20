using DelitaTrade.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Core.Contracts
{
    public interface ITraderService
    {
        Task<IEnumerable<TraderViewModel>> GetAllAsync();

        Task<int> CreateAsync(TraderViewModel traderViewModel);

        Task UpdateAsync(TraderViewModel traderViewModel);

        Task DeleteSafeAsync(TraderViewModel traderViewModel);
    }
}
