using DelitaTrade.Core.ViewModels;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.ViewModels;
using DelitaTrade.ViewModels.Controllers;
using DelitaTrade.ViewModels.Interfaces;
using DelitaTrade.WpfViewModels;
using System.ComponentModel;
using System.Windows;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class DayReportLoaderViewModel : ViewModelBase, IDayReportData
    {
        private const string _unselectDayReportColor = "Red";
        private const string _selectDayReportColor = "#FF3BEB23";

        private DateTime _date = DateTime.Now;

        private readonly DayReportListIdViewModel _dayReportListIdViewModel;
        private readonly IDayReportCrudController _dayReportCrudController;
        private readonly DayReportTotalsViewModel _dayReportTotalsViewModel;
        private readonly DayReportCommandsViewModel _dayReportCommandsViewModel;
        private DayReportViewModel? _dayReportViewModel;

        public DayReportLoaderViewModel(DayReportListIdViewModel dayReportListIdViewModel, IDayReportCrudController dayReportCrudController, UserController userController, DayReportTotalsViewModel dayReportTotalsViewModel, DayReportCommandsViewModel dayReportCommandsViewModel)
        {
            _dayReportListIdViewModel = dayReportListIdViewModel;
            _dayReportCrudController = dayReportCrudController;
            userController.UserLogIn += OnUserLogIn;
            _dayReportListIdViewModel.DayReportIdSelected += OnDayReportSelected;
            _dayReportTotalsViewModel = dayReportTotalsViewModel;
            _dayReportCommandsViewModel = dayReportCommandsViewModel;
            _dayReportCommandsViewModel.InitializedCommands(_dayReportCrudController, this);
            _dayReportCommandsViewModel.OnDayReportCreated += OnDayReportCreated;
            _dayReportCrudController.OnDeleted += OnDayReportUnSelected;
            DayReportTotalsViewModel.PropertyChanged += OnTransmissionDateChange;
            DayReportSelected += (d) => { };
            DayReportUnSelect += () => { };
        }

        public DayReportListIdViewModel DayReportListIdViewModel => _dayReportListIdViewModel;
        public DayReportTotalsViewModel DayReportTotalsViewModel => _dayReportTotalsViewModel;
        public DayReportCommandsViewModel DayReportCommandsViewModel => _dayReportCommandsViewModel;

        public string DayReportDate => _dayReportViewModel?.Date.Date.ToString("yyyy-MM-dd") ?? "Not Load";
        public string DayReportColor => _dayReportViewModel == null ? _unselectDayReportColor : _selectDayReportColor;

        public int CurrentDayReportId => _dayReportViewModel?.Id ?? 0;

        public event Action<DayReportViewModel> DayReportSelected;

        public event Action DayReportUnSelect;
                
        public DateTime Date
        {
            get => _date;
            set { _date = value; }
        }

        public bool HasDayReportLoad => _dayReportViewModel != null;


        public async void DayReportUpdate(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            if (_dayReportViewModel?.Id != invoiceViewModel!.DayReport!.Id) throw new InvalidOperationException(nameof(DayReportUpdate));
            DayReportTotalsViewModel.UpdateDayReport(await _dayReportCrudController.ReadDayReportByIdAsync(invoiceViewModel!.DayReport!.Id));
        }
          
        public async void DayReportUpdate(DayReportViewModel dayReport)
        {
            await _dayReportCrudController.UpdateDayReportAsync(dayReport);
        }

        public void AddInvoice(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _dayReportViewModel?.Invoices.Add(invoiceViewModel);
        }

        public void RemoveInvoice(Core.ViewModels.InvoiceViewModel invoiceViewModel)
        {
            _dayReportViewModel?.Invoices.Remove(invoiceViewModel);
        }

        private void OnUserLogIn(UserViewModel userViewModel)
        {
            if (DayReportListIdViewModel.IsInitialized == false)
            {
                DayReportListIdViewModel.Initialized(_dayReportCrudController);
            }
            else 
            {
                DayReportListIdViewModel.UpdateCollection(_dayReportCrudController);
            }
        }

        private async void OnDayReportSelected(int Id)
        {
            _dayReportViewModel = await _dayReportCrudController.ReadDayReportByIdAsync(Id);
            OnPropertyChange(nameof(DayReportDate));
            OnPropertyChange(nameof(HasDayReportLoad));
            OnPropertyChange(nameof(DayReportColor));
            DayReportSelected(_dayReportViewModel);
            DayReportTotalsViewModel.SelectDayReport(_dayReportViewModel);
        }

        private void OnDayReportCreated(DayReportViewModel dayReport)
        {
            _dayReportViewModel = dayReport;
            OnPropertyChange(nameof(DayReportDate));
            OnPropertyChange(nameof(HasDayReportLoad));
            OnPropertyChange(nameof(DayReportColor));
            DayReportSelected(_dayReportViewModel);
            DayReportTotalsViewModel.SelectDayReport(_dayReportViewModel);
        }

        private void OnDayReportUnSelected(WpfDayReportIdViewModel wpfDayReportId)
        {
            _dayReportViewModel = null;
            OnPropertyChange(nameof(DayReportDate));
            OnPropertyChange(nameof(HasDayReportLoad));
            OnPropertyChange(nameof(DayReportColor));
            DayReportUnSelect();
        }  
        
        private void OnTransmissionDateChange(object? sender, PropertyChangedEventArgs e)
        {
            if(_dayReportViewModel != null && e.PropertyName == nameof(DayReportTotalsViewModel.Date))
            {
                _dayReportViewModel.TransmissionDate = DayReportTotalsViewModel.Date;
                _dayReportCrudController.UpdateDayReportAsync(_dayReportViewModel);
            }
        }
    }
}
