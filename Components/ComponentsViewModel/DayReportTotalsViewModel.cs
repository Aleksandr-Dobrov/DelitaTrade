using DelitaTrade.Components.ComponentsCommands;
using DelitaTrade.Models;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace DelitaTrade.Components.ComponentsViewModel
{
    //Deprecated
    public class DayReportTotalsViewModel : ViewModelBase
    {
        private SearchBoxTextViewModel _vehicle;
        private SearchBoxTextViewModel _transmissionDate;
        private DelitaTradeDayReport _delitaTradeDayReport;
        private ObservableCollection<string> _vehicles;
        private string _exportImageSource = "Components\\ComponentAssets\\DayReport\\excelIcon.png";

        public DayReportTotalsViewModel(DelitaTradeDayReport delitaTradeDayReport)
        {
            _delitaTradeDayReport = delitaTradeDayReport;
            _vehicles = new ObservableCollection<string>();
            _vehicle = new SearchBoxTextViewModel(_vehicles, "Vehicle");
            _transmissionDate = new SearchBoxTextViewModel(CreateDates(DateTime.Now), "Date");
            PrintDayReport = new ExportDayReportCommand(_delitaTradeDayReport);
            _delitaTradeDayReport.VehiclesChanged += VehiclesUpdate;
            _delitaTradeDayReport.CurentDayReportSelect += UpdateDayReportData;
            _delitaTradeDayReport.TotalsChanged += TotalsChanged;
            _vehicle.PropertyChanged += OnVehicleViewModelChanged;
            _transmissionDate.PropertyChanged += OnTransmissionDateViewModelChanged;
            VehiclesUpdate();
        }

        public SearchBoxTextViewModel Vehicle => _vehicle;
                
        public SearchBoxTextViewModel TransmissionDate => _transmissionDate;
        public string TotalAmount => $"{_delitaTradeDayReport.TotalAmount:C}";        
        public string TotalIncome => $"{_delitaTradeDayReport.TotalIncome:C}";
        public string TotalExpenses => $"{_delitaTradeDayReport.TotalExpenses:C}";               
        public string TotalNonPay => $"{_delitaTradeDayReport.TotalNonPay:C}";               
        public string TotalOldInvoice => $"{_delitaTradeDayReport.TotalOldInvoice:C}";
        public string TotalWeight => $"{_delitaTradeDayReport.TotalWeight:F0} kg.";
        public string ExportImageSource => _exportImageSource.GetFullFilePathExt();
        
        public ICommand AddNewVehicle { get; }

        public ICommand PrintDayReport { get; }

        private void VehiclesUpdate()
        {
            _vehicles.Clear();
            foreach (var vehicle in _delitaTradeDayReport.GetAllVehicles())
            {
                _vehicles.Add(vehicle);
            }
        }

        private void UpdateDayReportData()
        {
            _vehicle.Item = _delitaTradeDayReport.Vehicle;
            _transmissionDate.Item = _delitaTradeDayReport.TransmissionDate;
        }

        private void TotalsChanged()
        {
            OnPropertyChange(nameof(TotalAmount));
            OnPropertyChange(nameof(TotalIncome));
            OnPropertyChange(nameof(TotalNonPay));
            OnPropertyChange(nameof(TotalOldInvoice));
            OnPropertyChange(nameof(TotalExpenses));
            OnPropertyChange(nameof(TotalWeight));
        }

        private void OnVehicleViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Vehicle.Item) && Vehicle.Item != null)
            {
                if (_delitaTradeDayReport.DayReport != null)
                {
                    Vehicle.TextColor = "Black";

                    if (_delitaTradeDayReport.IsValidLicencePlate(Vehicle.Item))
                    {
                        if (_delitaTradeDayReport.DayReport.Vehicle != Vehicle.Item)
                        { 
                            MessageBoxResult result = MessageBox.Show("Add vehicle to day report?",
                                         "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if (result == MessageBoxResult.Yes)
                            {
                                _delitaTradeDayReport.AddVehicle(Vehicle.Item.ToUpper());
                            }                        
                        }                    
                    }                    
                    else
                    {
                        Vehicle.TextColor = "Red";
                    }
                }
                else
                {
                    Vehicle.TextColor = "Red";
                }
            }
        }

        private void OnTransmissionDateViewModelChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(TransmissionDate.Item))
            {
                _delitaTradeDayReport.SetTransmissionDate(TransmissionDate.Item);
            }
        }

        private ObservableCollection<string> CreateDates(DateTime date)
        {            
            var dates = new ObservableCollection<string>();
            
            for (int i = 0; i < 10; i++)
            {
                dates.Add($"{date.Date.AddDays(i):dd-MM-yyyy}");
            }
            return dates;
        }
    }
}
