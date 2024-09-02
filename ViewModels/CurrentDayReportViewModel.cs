using DelitaTrade.Models;

namespace DelitaTrade.ViewModels
{
    public class CurrentDayReportViewModel : ViewModelBase
    {
        private readonly DayReport _currentDayReport;

        public CurrentDayReportViewModel(DayReport currentDayReport)
        {
            _currentDayReport = currentDayReport;
        }

        public string DayReportId => _currentDayReport.DayReportID;

        public decimal TotalAmaunt => _currentDayReport.TotalAmaunt;

        public decimal TotalIncome => _currentDayReport.TotalIncome;

        public decimal TotalExpenses => _currentDayReport.TotalExpenses;

        public decimal TotalNonPay => _currentDayReport.TotalNonPay;

        public decimal TotalOldInvoice => _currentDayReport.TotalOldInvoice;

    }
}
