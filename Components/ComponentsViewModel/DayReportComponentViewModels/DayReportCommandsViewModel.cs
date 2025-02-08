using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Common;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels.Interfaces;
using DelitaTrade.WpfViewModels;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportCommandsViewModel
    {
        private const string _deleteDayReportButtonImage = "Components\\ComponentAssets\\DayReport\\delete-file_40456.png";
        private const string _createDayReportButtonImage = "Components\\ComponentAssets\\DayReport\\add-document.png";

        private bool _isInitialized;

        private IDayReportCrudController _crudController = null!;
        private IDayReportData _dayReportData = null!;

        public DayReportCommandsViewModel()
        {
            Create = new DefaultCommand(CreateDayReport);
            OnDayReportCreated += (d) => { };
        }
        public string DeleteDayReportButtonImage => _deleteDayReportButtonImage.GetFullFilePathExt();
        public string CreateDayReportButtonImage => _createDayReportButtonImage.GetFullFilePathExt();

        public bool IsInitialized => _isInitialized;

        public event Action<DayReportViewModel> OnDayReportCreated;

        public ICommand Create { get; private set; }
        public ICommand Delete { get; private set; }

        public void InitializedCommands(IDayReportCrudController crudController, IDayReportData dayReportData)
        {
            _crudController = crudController;
            _dayReportData = dayReportData;
            Delete = new DefaultCommand(DeleteDayReport, CanDelete, dayReportData, nameof(dayReportData.HasDayReportLoad));
            _isInitialized = true;
        }

        private async Task CreateDayReport()
        {
            try
            {
                IsInitializedCheck();
                var newDayReport = await _crudController.CreateDayReportAsync(new WpfDayReportViewModel() { Date = _dayReportData.Date });
                OnDayReportCreated(newDayReport);
            }
            catch (InvalidOperationException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
            }
        }

        private async Task DeleteDayReport()
        {
            try
            {
                IsInitializedCheck();
                await _crudController.DeleteDayReportByIdAsync(_dayReportData.CurrentDayReportId);
            }
            catch (InvalidOperationException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error);
            }
        }

        private bool CanDelete()
        {
            return _dayReportData.HasDayReportLoad;
        }

        private void IsInitializedCheck()
        {
            if (_isInitialized == false) throw new InvalidOperationException(ExceptionMessages.NotInitialized(nameof(DayReportCommandsViewModel)));
        }
    }
}
