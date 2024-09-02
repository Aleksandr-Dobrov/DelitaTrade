using DelitaTrade.Commands;
using DelitaTrade.ViewModels;
using DelitaTrade.Models;
using System.ComponentModel;
using System.Windows;
using DelitaTrade.Models.Loggers;

namespace DelitaTrade.Components.ComponentsCommands
{
    class PrintDayReportCommand : CommandBase
    {  
        private DelitaTradeDayReport _delitaTradeDayReport;
       
        public PrintDayReportCommand(DelitaTradeDayReport delitaTradeDayReport)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _delitaTradeDayReport.CurentDayReportSelect += CurrentDayReportChanged;
            _delitaTradeDayReport.CurrentDayReportUnselected += CurrentDayReportChanged;
            _delitaTradeDayReport.DayReportDataChanged += CurrentDayReportChanged;
            _delitaTradeDayReport.VehiclesChanged += CurrentDayReportChanged;
            _delitaTradeDayReport.TransmisionDateChange += CurrentDayReportChanged;
        }

        private void CurrentDayReportChanged()
        {
            OnCanExecuteChanged();
        }

        public override bool CanExecute(object? parameter)
        {
            return _delitaTradeDayReport?.DayReport?.InvoicesCount > 0
                &&_delitaTradeDayReport.CurentDayReportId != null
                && _delitaTradeDayReport.TransmissionDate != null
                && _delitaTradeDayReport.Vehicle != null
               && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            MessageBoxResult result = MessageBoxResult.Yes;
            if (_delitaTradeDayReport.IsEnoughMoney() == false)
            {
                result = MessageBox.Show("The report money is insufficient! Export anyway?", "Caution!"
                                                            , MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            }
            if (result == MessageBoxResult.Yes)
            {
                _delitaTradeDayReport.ExportExcelDayReport();
            }
            else
            {
                new MessageBoxLogger().Log("Day report is not exported!", Logger.LogLevel.Information);
            }
        }
    }
}
