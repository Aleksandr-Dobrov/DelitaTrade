using DelitaTrade.Models;
using DelitaTrade.ViewModels;
using System.Windows;

namespace DelitaTrade.Commands
{
    public class DeleteDayReportCommand : CommandBase
    {
        private readonly DelitaTradeDayReport _dayReport;

        private readonly DayReportsViewModel _dayReportsViewModel;

        public DeleteDayReportCommand(DelitaTradeDayReport dayReport, DayReportsViewModel dayReportsViewModel)
        {
            _dayReport = dayReport;
            _dayReportsViewModel = dayReportsViewModel;
            _dayReport.CurentDayReportSelect += OnDayReportChanged;
            _dayReport.CurrentDayReportUnselected += OnDayReportChanged;
        }

        private void OnDayReportChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return _dayReportsViewModel.LoadDayReportId.Length == 10 
                && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            if (Agreement("Delete day report", _dayReportsViewModel.DayReportId))
            {
                _dayReport.DeleteDayReport(_dayReportsViewModel.LoadDayReportId);                
            }
        }
    }
}
