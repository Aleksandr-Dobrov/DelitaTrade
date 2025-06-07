using DelitaTrade.Commands;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Core.Extensions;
using System.Windows;
using System.Windows.Input;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Core.Exporters.ExportedModels;
using DelitaTrade.Core.Factories;
using DelitaTrade.ViewModels;
using System.Diagnostics;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportExportCommandViewModel : ViewModelBase
    {
        private string _exportImageSource = "Components\\ComponentAssets\\DayReport\\excelIcon.png";
        private readonly InternetProviderViewModel _internetProviderViewModel;
        private readonly DayReportExporter _dayReportExporter;
        private DayReportViewModel? _currentDayReport;
        private bool _exportInProgress;

        public DayReportExportCommandViewModel(InternetProviderViewModel internetProviderViewModelProvider, DayReportExporter dayReportBuilder)
        {
            _internetProviderViewModel = internetProviderViewModelProvider;
            _dayReportExporter = dayReportBuilder;
            _dayReportExporter.ExportStart += OnExportStarted;
            _dayReportExporter.ExportCompleted += OnExportFinished;
            _dayReportExporter.ExportFileCreate += OnExportFileCreated;
            ExportDayReport = new DefaultCommand(ExportDayReportAsync, CanExportDayReport, [_internetProviderViewModel, this], nameof(_internetProviderViewModel.IsConnected), nameof(_currentDayReport), nameof(ExportInProgress));
        }

        public string ExportImageSource => _exportImageSource.GetFullFilePathExt();
        public bool ExportInProgress 
        {
            get => _exportInProgress;
            set
            {
                _exportInProgress = value;
                OnPropertyChange();
            } 
        }
        public ICommand ExportDayReport { get; }

        public void OnDayReportSelected(DayReportViewModel dayReportViewModel)
        {
            _currentDayReport = dayReportViewModel;
            OnPropertyChange(nameof(_currentDayReport));
        }

        public void OnDayReportUnselected()
        {
            _currentDayReport = null;
            OnPropertyChange(nameof(_currentDayReport));
        }

        private async Task ExportDayReportAsync()
        {
            if (_currentDayReport == null || _currentDayReport.Vehicle == null) return;
            bool result = true;
            if (_currentDayReport.IsEnoughCash() == false)
            {
                result = Agreement("The report money is insufficient!", "Export anyway");
            }
            if (result)
            {
                await _dayReportExporter.ExportDayReport(_currentDayReport.CreateExportedDayReport(), "../../../DayReportsDataBase/ExportFiles/ExportedDayReport.xlsx", MessageToCloseExportFile);
            }
            else
            {
                new MessageBoxLogger().Log("Day report is not exported!", Logger.LogLevel.Information);
            }
        }

        private bool CanExportDayReport()
        {
            return _internetProviderViewModel.IsConnected &&
                   _currentDayReport != null &&
                   ExportInProgress == false;
        }
        private static bool Agreement(string message, string target)
        {
            MessageBoxResult result = MessageBox.Show($"{message} - {target} ?", message, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) return true;
            return false;
        }

        private bool MessageToCloseExportFile(string message)
        {
            MessageBoxResult result = MessageBox.Show(message,
                                                        "Export", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                return true;
            }
            return false;
        }

        private void OnExportStarted()
        {
            ExportInProgress = true;
        }
        private void OnExportFinished() 
        {
            ExportInProgress = false;
        }

        private void OnExportFileCreated(string filePath) 
        {
            MessageBoxResult boxResult = MessageBox.Show($"Day report exported successful.{Environment.NewLine}Open file?", "Exporter"
                                                             , MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.Yes)
            {
                Process.Start("explorer.exe", filePath);
            }
        }
    }
}
