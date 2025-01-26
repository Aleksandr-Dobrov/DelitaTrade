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
        Task<IEnumerable<ReturnProtocolViewModel>> GetAllAsync(UserViewModel user);

        Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(UserViewModel user, string arg);

        Task<int> CreateProtocolAsync(ReturnProtocolViewModel protocolViewModel);

        Task DeleteProtocol(int protocolId);

    }
}
