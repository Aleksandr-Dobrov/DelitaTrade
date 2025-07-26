using DelitaTrade.Core.ViewModels;
using DelitaTrade.WpfViewModels;

namespace DelitaTrade.ViewModels.Interfaces
{
    public interface IDayReportCrudController
    {
        Task<IEnumerable<DayReportHeaderViewModel>> ReadAllHeaders();
        Task<DayReportViewModel> ReadDayReportByIdAsync(int Id);
        Task<DayReportBanknotesViewModel> ReadDayReportBanknotesByIdAsync(int Id);
        Task<DayReportViewModel> CreateDayReportAsync(WpfDayReportViewModel dayReport);
        Task UpdateDayReportAsync(DayReportViewModel dayReport);
        Task DeleteDayReportByIdAsync(int dayReportId);

        event Action<DayReportViewModel> OnCreated;

        event Action<WpfDayReportIdViewModel> OnDeleted;
    }
}
