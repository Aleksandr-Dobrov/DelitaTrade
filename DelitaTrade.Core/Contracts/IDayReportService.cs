using DelitaTrade.Core.ViewModels;
using DelitaTrade.Core.ViewModels.DayReportModels;
using DelitaTrade.Infrastructure.Data.Models;

namespace DelitaTrade.Core.Contracts
{
    public interface IDayReportService
    {
        Task<IEnumerable<DayReportHeaderViewModel>> GetAllDatesAsync(UserViewModel user);
        Task<IEnumerable<SimpleDayReportViewModel>> GetSimpleFilteredAsync(UserViewModel user, string? reporterUseName, DateTime? startDate, DateTime? endDate);   
        Task<IEnumerable<UserViewModel>> GetAllUsersWhitDayReports(UserViewModel user);
        Task<IEnumerable<UserViewModel>> GetAllDrivers(UserViewModel user);
        Task<DayReportViewModel> GetByIdAsync(UserViewModel user, int id);
        Task<DayReportBanknotesViewModel> GetBanknotesReadonlyAsync(UserViewModel user, int id);
        Task<DayReportViewModel> CreateAsync(DayReportViewModel dayReport);
        Task UpdateAsync(DayReportViewModel dayReport);
        Task DeleteAsync(UserViewModel userViewModel, int id);
    }
}
