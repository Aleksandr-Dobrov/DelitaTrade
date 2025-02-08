using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models;
using DelitaTrade.ViewModels;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportTotalsViewModel : ViewModelBase
    {
        private DayReportViewModel? _dayReportViewModel;
        public string TotalAmount => $"{_dayReportViewModel?.TotalAmount:C}";
        public string TotalIncome => $"{_dayReportViewModel?.TotalIncome:C}";
        public string TotalExpenses => $"{_dayReportViewModel?.TotalExpense:C}";
        public string TotalNonPay => $"{_dayReportViewModel?.TotalNotPay:C}";
        public string TotalOldInvoice => $"{_dayReportViewModel?.TotalOldInvoice:C}";
        public string TotalWeight => $"{_dayReportViewModel?.TotalWeight:F0} kg.";

        public void SelectDayReport(DayReportViewModel dayReportViewModel)
        {
            _dayReportViewModel = dayReportViewModel;
            TotalsChanged();
        }

        public void UpdateDayReport(DayReportViewModel dayReportViewModel)
        {
            if (_dayReportViewModel == null) return;
            _dayReportViewModel.Update(dayReportViewModel);
            TotalsChanged();
        }

        private void TotalsChanged()
        {
            OnPropertyChange(nameof(TotalAmount));
            OnPropertyChange(nameof(TotalIncome));
            OnPropertyChange(nameof(TotalNonPay));
            OnPropertyChange(nameof(TotalOldInvoice));
            OnPropertyChange(nameof(TotalExpenses));
            OnPropertyChange(nameof(TotalWeight));
        }
    }
}
