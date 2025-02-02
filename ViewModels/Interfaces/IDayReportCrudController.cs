using DelitaTrade.Core.ViewModels;
using DelitaTrade.WpfViewModels;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface IDayReportCrudController
    {
        Task<IEnumerable<DayReportViewModel>> ReadAll();
        Task<DayReportViewModel> ReadDayReportByIdAsync(int Id);
        Task<DayReportViewModel> CreateDayReportAsync(WpfDayReportViewModel dayReport);
        Task<DayReportViewModel> UpdateDayReportAsync(DayReportViewModel dayReport);
        Task DeleteDayReportByIdAsync(int dayReportId);

        event Action<DayReportViewModel> OnCreated;

        event Action<WpfDayReportIdViewModel> OnDeleted;
    }
}
