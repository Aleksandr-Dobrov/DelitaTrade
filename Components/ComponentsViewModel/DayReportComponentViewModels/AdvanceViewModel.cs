using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels;
using static DelitaTrade.Common.ExceptionMessages;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class AdvanceViewModel : ViewModelBase
    {
        private bool _isEditable = false;
        private readonly VehiclesListViewModel _vehiclesListViewModel;
        private readonly DayReportExportCommandViewModel _dayReportExportViewModel;
        private DayReportViewModel? _currentDayReport;

        public AdvanceViewModel(VehiclesListViewModel vehiclesListViewModel, DayReportExportCommandViewModel dayReportExportViewModel)
        {
            _vehiclesListViewModel = vehiclesListViewModel;
            VehiclesListViewModel.VehiclesViewModel.Name = "Vehicle";
            VehiclesListViewModel.VehicleSelected += OnVehicleSelected;
            DayReportChange += (d) => { };
            _dayReportExportViewModel = dayReportExportViewModel;
        }

        public VehiclesListViewModel VehiclesListViewModel => _vehiclesListViewModel;
        public DayReportExportCommandViewModel DayReportExportViewModel => _dayReportExportViewModel;

        public bool IsEditable => _isEditable;


        public event Action<DayReportViewModel> DayReportChange;

        public void OnDayReportSelected(DayReportViewModel dayReportViewModel)
        {
            _currentDayReport = dayReportViewModel;
            _isEditable = true;
            OnPropertyChange(nameof(IsEditable));
            if (dayReportViewModel.Vehicle != null)
            {
                VehiclesListViewModel.VehiclesViewModel.TextValue = dayReportViewModel.Vehicle.LicensePlate;
            }
            else
            {
                VehiclesListViewModel.VehiclesViewModel.TextValue = string.Empty;
            }
        }

        public void OnDayReportUnselected()
        {
            _currentDayReport = null;
            _isEditable = false;
            OnPropertyChange(nameof(IsEditable));
            VehiclesListViewModel.VehiclesViewModel.TextValue = string.Empty;
        }

        private void OnVehicleSelected(VehicleViewModel vehicleViewModel)
        {
            if (_currentDayReport == null) throw new ArgumentNullException(NotFound(nameof(DayReportViewModel)));
            _currentDayReport.Vehicle = vehicleViewModel;
            DayReportChange(_currentDayReport);
        }
    }
}
