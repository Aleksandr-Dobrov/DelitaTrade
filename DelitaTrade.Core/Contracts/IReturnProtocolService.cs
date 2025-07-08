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

        Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(UserViewModel user, string[] arg);

        Task<IEnumerable<ReturnProtocolViewModel>> GetFilteredAsync(UserViewModel user, string[] arg, DateTime startDate, DateTime endDate);

        Task<IEnumerable<SimpleReturnProtocolViewModel>> GetSimpleFilteredAsync(UserViewModel user, string? trader, string? companyObject, DateTime? startDate, DateTime? endDate);
        
        Task<DetailReturnProtocolViewModel?> GetByIdAsync(UserViewModel user, int id);

        Task<EditableReturnProtocolViewModel?> GetEditableByIdAsync(UserViewModel user, int id);

        Task<int> CreateProtocolAsync(ReturnProtocolViewModel protocolViewModel);

        Task UpdateProtocolAsync(ReturnProtocolViewModel returnProtocol);

        Task ApproveAsync(ReturnProtocolApproveModel detailReturnProtocol, UserViewModel user);

        Task<bool> IsApproved(int protocolId);

        Task DeleteProtocolAsync(int protocolId);
    }
}
