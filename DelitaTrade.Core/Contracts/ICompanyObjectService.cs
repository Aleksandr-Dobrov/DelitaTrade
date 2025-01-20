using DelitaTrade.Core.ViewModels;

namespace DelitaTrade.Core.Contracts
{
    public interface ICompanyObjectService
    {
        Task<int> CreateAsync(CompanyObjectDeepViewModel companyObject);
        Task<IEnumerable<CompanyObjectViewModel>> GetAllAsync();
        Task<CompanyObjectDeepViewModel> GetDetailedByIdAsync(int companyObjectId);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string arg, int limit);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string arg, int companyId, int limit);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredByNameAsync(string arg, int limit);
        Task UpdateAsync(CompanyObjectViewModel companyObject);
        Task DeleteSafeAsync(CompanyObjectViewModel companyObjectId);
    }
}
