using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.Extensions;
using DelitaTrade.Core.Services;
using DelitaTrade.Core.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class VehiclesListViewModel
    {
        private readonly IVehicleService _vehicleService;
        private readonly EntityBaseControllerViewModel<VehicleViewModel> _listViewModel = new();

        public VehiclesListViewModel(IVehicleService service)
        {
            _vehicleService = service;
            VehiclesViewModel.PropertyChanged += OnViewModelPropertyChange;
            Task.Run(async () => { await GetAllAsync();});
            VehicleSelected += (v) => { };
        }

        public event Action<VehicleViewModel> VehicleSelected;

        public async Task GetAllAsync()
        {
            _listViewModel.UpdateItems((await _vehicleService.AllAsync()).OrderBy(v => v.LicensePlate));            
        }

        public EntityBaseControllerViewModel<VehicleViewModel> VehiclesViewModel => _listViewModel;
        private async void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(VehiclesViewModel.TextValue))
            {
                await AddVehicleAsync(VehiclesViewModel.TextValue);
            }
            if (e.PropertyName == nameof(VehiclesViewModel.Value.Value) && VehiclesViewModel.Value.Value != null)
            {
                VehicleSelected(VehiclesViewModel.Value.Value);
            }
        }

        private async Task AddVehicleAsync(string licensePlate)
        {
            if (licensePlate.IsValidLicensePlate() && _listViewModel.Items.Any(v => v.LicensePlate == licensePlate) == false)
            {   
                _listViewModel.Add(await _vehicleService.CreateAsync(new VehicleViewModel { LicensePlate = licensePlate.ToUpper() }));
            }
        }
    }
}
