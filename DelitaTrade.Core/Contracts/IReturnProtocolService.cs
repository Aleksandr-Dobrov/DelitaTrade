using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface IReturnProtocolService
    {
        Task<IEnumerable<ReturnProtocolViewModel>> GetAllAsync(int userId);

        Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(int userId, string arg);

        Task<int> CreateProtocolAsync(ReturnProtocolViewModel protocolViewModel);

        Task DeleteProtocol(int protocolId);

    }
}
