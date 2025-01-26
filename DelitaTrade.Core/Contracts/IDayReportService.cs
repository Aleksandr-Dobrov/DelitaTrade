using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Contracts
{
    public interface IDayReportService
    {
        Task<IEnumerable<DayReportViewModel>> GetAllDatesAsync(UserViewModel user);
        Task<IEnumerable<DayReportViewModel>> GetAllFilteredAsync(UserViewModel user, string filter, int limit);
        Task<DayReportViewModel> GetByIdAsync(UserViewModel user, int id);
        Task<int> CreateAsync(DayReportViewModel dayReport);
        Task UpdateAsync(DayReportViewModel dayReport);
        Task DeleteAsync(UserViewModel user, int id);
    }
}
