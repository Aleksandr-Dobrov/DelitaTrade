using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.ExpenseModels;

namespace DelitaTrade.Core.Contracts
{
    public interface ICompanyObjectService
    {
        Task<int> CreateAsync(CompanyObjectDeepViewModel companyObject);
        Task<IEnumerable<CompanyObjectViewModel>> GetAllAsync();
        Task<CompanyObjectDeepViewModel> GetDetailedByIdAsync(int companyObjectId);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string arg, int limit);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string[] arg, int limit);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredAsync(string arg, int companyId, int limit);
        Task<IEnumerable<CompanyObjectViewModel>> GetFilteredByNameAsync(string arg, int limit);
        Task<IEnumerable<ExpenseDropDownModel>> GetAllExpensesAsync(int vehicleId);
        Task UpdateAsync(CompanyObjectViewModel companyObject);
        Task UpdateIsBankStatus(CompanyObjectViewModel companyObject);
        Task DeleteSoftAsync(CompanyObjectViewModel companyObjectId);
    }
}
