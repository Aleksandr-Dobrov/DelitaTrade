using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Core.Extensions;
using System.Windows;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportExportCommandViewModel
    {
        private string _exportImageSource = "Components\\ComponentAssets\\DayReport\\excelIcon.png";
        private readonly InternetProvider _internetProvider;
        private DayReportViewModel _currentDayReport;

        public DayReportExportCommandViewModel(InternetProvider internetProvider)
        {
            ExportDayReport = new DefaultCommand(ExportDayReportAsync);
            _internetProvider = internetProvider;
        }

        public string ExportImageSource => _exportImageSource.GetFullFilePathExt();
        public ICommand ExportDayReport { get; }

        private async Task ExportDayReportAsync()
        {
            bool result = true;
            if (_currentDayReport.IsEnoughCash() == false)
            {
                result = Agreement("The report money is insufficient!", "Export anyway?");
            }
            if (result)
            {
                //_delitaTradeDayReport.ExportDayReportAsync();
            }
            else
            {
                new MessageBoxLogger().Log("Day report is not exported!", Logger.LogLevel.Information);
            }
        }

        private bool CanExportDayReport()
        {
            return true;
        }
        private bool Agreement(string message, string target)
        {
            MessageBoxResult result = MessageBox.Show($"{message} - {target} ?", message, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }
    }
}
