using DelitaTrade.Commands;
using DelitaTrade.Models;
using System.Windows;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Models.DataProviders;

namespace DelitaTrade.Components.ComponentsCommands
{
    class ExportDayReportCommand : CommandBase
    {
        private DelitaTradeDayReport _delitaTradeDayReport;
        private InternetProvider _internetProvider;
        private bool _isComplete = true;

        public ExportDayReportCommand(DelitaTradeDayReport delitaTradeDayReport)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _internetProvider = new InternetProvider();
            _delitaTradeDayReport.CurentDayReportSelect += CurrentDayReportChanged;
            _delitaTradeDayReport.CurrentDayReportUnselected += CurrentDayReportChanged;
            _delitaTradeDayReport.DayReportDataChanged += CurrentDayReportChanged;
            _delitaTradeDayReport.VehiclesChanged += CurrentDayReportChanged;
            _delitaTradeDayReport.TransmisionDateChange += CurrentDayReportChanged;
            _internetProvider.NetworkStatusChange += CurrentDayReportChangedAsync;
            _delitaTradeDayReport.ExportCompleted += IsCompleted;
            _delitaTradeDayReport.ExportCompleted += CurrentDayReportChanged;
            _delitaTradeDayReport.ExportStart += ExportStarted;
            _delitaTradeDayReport.ExportStart += CurrentDayReportChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            return _delitaTradeDayReport?.DayReport?.InvoicesCount > 0
                && _delitaTradeDayReport.CurentDayReportId != null
                && _delitaTradeDayReport.TransmissionDate != null
                && _delitaTradeDayReport.Vehicle != null
                && _isComplete
                && _internetProvider.CheckForInternetConnection()
               && base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            bool result = true;
            if (_delitaTradeDayReport.IsEnoughMoney() == false)
            {
                result = Agreement("The report money is insufficient!", "Export anyway?");
            }
            if (result)
            {                
                _delitaTradeDayReport.ExportDayReport();
            }
            else
            {
                new MessageBoxLogger().Log("Day report is not exported!", Logger.LogLevel.Information);
            }
        }

        private void CurrentDayReportChanged()
        {
            OnCanExecuteChanged();
        }

        private void CurrentDayReportChangedAsync()
        {
            Application.Current.Dispatcher.Invoke(new Action(OnCanExecuteChanged));
        }

        private void IsCompleted()
        {
            _isComplete = true;
        }

        private void ExportStarted()
        {
            _isComplete = false;
        }
    }    
}
