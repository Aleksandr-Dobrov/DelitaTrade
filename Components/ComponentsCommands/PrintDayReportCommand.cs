using DelitaTrade.Commands;
using DelitaTrade.Models;
using System.Windows;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Models.DataProviders;

namespace DelitaTrade.Components.ComponentsCommands
{
    class PrintDayReportCommand : CommandBase
    {  
        private DelitaTradeDayReport _delitaTradeDayReport;
        private InternetProvider _internetProvider;
       
        public PrintDayReportCommand(DelitaTradeDayReport delitaTradeDayReport)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _internetProvider = new InternetProvider();
            _delitaTradeDayReport.CurentDayReportSelect += CurrentDayReportChanged;
            _delitaTradeDayReport.CurrentDayReportUnselected += CurrentDayReportChanged;
            _delitaTradeDayReport.DayReportDataChanged += CurrentDayReportChanged;
            _delitaTradeDayReport.VehiclesChanged += CurrentDayReportChanged;
            _delitaTradeDayReport.TransmisionDateChange += CurrentDayReportChanged;
            _internetProvider.NetworkStatusChange += CurrentDayReportChangedAsync;
        }
       
        private void CurrentDayReportChanged()
        {
            OnCanExecuteChanged();
        }

        private void CurrentDayReportChangedAsync()
        {
            Application.Current.Dispatcher.Invoke(new Action(OnCanExecuteChanged));
        }

        public override bool CanExecute(object? parameter)
        {
            return _delitaTradeDayReport?.DayReport?.InvoicesCount > 0
                &&_delitaTradeDayReport.CurentDayReportId != null
                && _delitaTradeDayReport.TransmissionDate != null
                && _delitaTradeDayReport.Vehicle != null
                && _internetProvider.CheckForInternetConnection()
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
