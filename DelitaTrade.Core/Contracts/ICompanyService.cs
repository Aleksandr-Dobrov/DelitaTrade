using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface ICompanyService
    {
        Task<IEnumerable<CompanyViewModel>> GetAllAsync();
        Task<IEnumerable<CompanyViewModel>> GetFilteredByNameAsync(string arg, int limit);
        Task<IEnumerable<CompanyViewModel>> GetFilteredAsync(string arg, int limit);
        Task<CompanyViewModel> GetDetailedCompanyByIdAsync(int companyId);
        Task<int> CreateAsync(CompanyViewModel company);
        Task UpdateAsync(CompanyViewModel company);
        Task DeleteSoftAsync(CompanyViewModel company);
    }
}
