using DelitaTrade.Commands;
using DelitaTrade.Core.Exporters.ExcelExporters;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using DelitaTrade.Core.Interfaces;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
using DelitaTrade.Core.Factories;

namespace DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels
{
    public class ReturnProtocolExportCommandsViewModel : ViewModelBase
    {
        private string _exportImageSource = "Components\\ComponentAssets\\DayReport\\excelIcon.png";
        private readonly InternetProviderViewModel _internetProviderViewModel;
        private readonly ReturnProtocolExporter _ReturnProtocolExporter;
        private ReturnProtocolViewModel? _currentReturnProtocol;
        private bool _exportInProgress;

        public ReturnProtocolExportCommandsViewModel(InternetProviderViewModel internetProviderViewModelProvider, ReturnProtocolExporter returnProtocolExporter)
        {
            _internetProviderViewModel = internetProviderViewModelProvider;
            _ReturnProtocolExporter = returnProtocolExporter;
            _ReturnProtocolExporter.ExportStart += OnExportStarted;
            _ReturnProtocolExporter.ExportCompleted += OnExportFinished;
            _ReturnProtocolExporter.ExportFileCreate += OnExportFileCreated;
            ExportDayReport = new DefaultCommand(ExportDayReportAsync, CanExportDayReport, [_internetProviderViewModel, this], nameof(_internetProviderViewModel.IsConnected), nameof(_currentReturnProtocol), nameof(ExportInProgress));
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

        public void OnReturnProtocolSelected(ReturnProtocolViewModel returnProtocolViewModel)
        {
            _currentReturnProtocol = returnProtocolViewModel;
            OnPropertyChange(nameof(_currentReturnProtocol));
        }

        public void OnDayReportUnselected(ReturnProtocolViewModel? returnProtocolViewModel)
        {
            _currentReturnProtocol = null;
            OnPropertyChange(nameof(_currentReturnProtocol));
        }

        private async Task ExportDayReportAsync()
        {
            if (_currentReturnProtocol == null) return;
            try
            {
                await _ReturnProtocolExporter.ExportReturnProtocol(_currentReturnProtocol.CreateExportedReturnProtocol(), "../../../DayReportsDataBase/ExportFiles/ReturnProtocol.xlsx", MessageToCloseExportFile);
            }
            catch (ArgumentNullException ex)
            {
                new MessageBoxLogger().Log(ex.Message, Logger.LogLevel.Error).Log(ex.Message, Logger.LogLevel.Error);
            }
        }

        private bool CanExportDayReport()
        {
            return _internetProviderViewModel.IsConnected &&
                   _currentReturnProtocol != null &&
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
            MessageBoxResult boxResult = MessageBox.Show($"Return protocol exported successful.{Environment.NewLine}Open file?", "Exporter"
                                                             , MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.Yes)
            {
                Process.Start("explorer.exe", filePath);
            }
        }
    }
}
