using DelitaTrade.Models.Builder;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Models.Configurations;
using System.Diagnostics;
using System.Windows;
using DelitaTrade.Services;
using DelitaTrade.Models.Interfaces.DayReports;
using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.DataBases;
using System.Configuration;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.Extensions.Configuration;

namespace DelitaTrade.Models
{
    public class DelitaTradeDayReport : IDayReportDataBase
    {
        private readonly DelitaSoundService _soundService;
        private DayReport _dayReport;
        private DayReportDataBase _dayReportData;
        private DayReportBuilder _dayReportBuilder;
        private readonly Configuration _appConfig;

        private decimal _totalAmound;
        private decimal _totalIncome;
        private decimal _totalExpenses;
        private decimal _totalNonPay;
        private decimal _totalOldInvoice;
        private double _totalWeight;
        private string _transmissionDate;

        public DelitaTradeDayReport(DelitaSoundService soundPlayer, IDBProvider dBProvider, Configuration appConfig, IConfiguration configuration)
        {
            _soundService = soundPlayer;
            _dayReportData = new DayReportDataBase(this, dBProvider, configuration);            
            _dayReportData.DayReportsIdChanged += OnDayReportsIdChanged;
            CurentDayReportSelect += SetTotalsToDayReportVievModel;
            DayReportDataChanged += SetTotalsToDayReportVievModel;
            CurentDayReportSelect += SetTransmissionDateToDayReportViewModel;
            CurrentDayReportUnselected += ResetTotals;
            MoneyChanged += PlayMoneyChangeSound;
            ExportCompleted += () => { };
            ExportStart += () => { };
            ExportFileCreate += (str) => { };
            _dayReportBuilder = new DayReportBuilder("../../../Models/Exporters/DayReport.xlsx", "../../../DayReportsDataBase/ExportFiles/ExportedDayReport.xlsx");
            _appConfig = appConfig;
        }

        public event System.Action DayReportDataChanged;
        public event Action<string> AddInvoiceToDataBase;
        public event System.Action CurentDayReportSelect;
        public event System.Action CurrentDayReportUnselected;
        public event System.Action DayReportsIdChanged;
        public event System.Action VehiclesChanged;
        public event System.Action TransmisionDateChange;
        public event System.Action TotalsChanged;
        public event System.Action MoneyChanged;
        public event System.Action ExportCompleted;
        public event System.Action ExportStart;
        public event System.Action<string> ExportFileCreate;

        public IDayReportIdDataBese DayReportIdDataBese => _dayReportData;
        public DayReport DayReport => _dayReport;
        public DelitaSoundService DelitaSoundService => _soundService;  
        public Configuration AppConfig => _appConfig;
        public string CurentDayReportId => DayReport?.DayReportID;
        public string Vehicle => _dayReport?.Vehicle;
          
        public decimal TotalAmount
        {
            get => _totalAmound;
            set
            {
                _totalAmound = value;
            }
        }

        public decimal TotalIncome
        {
            get => _totalIncome;
            set
            {
                _totalIncome = value;
            }

        }
        public decimal TotalExpenses
        {
            get => _totalExpenses;
            set
            {
                _totalExpenses = value;
            }
        }

        public double TotalWeight
        {
            get => _totalWeight;
            private set
            {
                _totalWeight = value;
            }
        }

        public decimal TotalNonPay
        {
            get => _totalNonPay;
            set
            {
                _totalNonPay = value;
            }
        }

        public decimal TotalOldInvoice
        {
            get => _totalOldInvoice;
            set
            {
                _totalOldInvoice = value;
            }
        }

        public string TransmissionDate
        {
            get => _dayReport?.TransmissionDate;
            set
            {
                _transmissionDate = value;
            }
        }

        public async void ExportDayReportAsync()
        {
            try
            {
                ExportStart();
                var t = Task.Factory.StartNew(() => _dayReportBuilder.CreateDayReport(_dayReportData.LoadCopyDayReport()));
                await RiseEventWhenExportCompleted(t);
            }
            catch (Exception ex)
            {
                new FileLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
                ExportCompleted();
            }
        }

        public bool IsEnoughMoney()
        {
            return _dayReport.CheckMoneyStatus();
        }

        public void CreateDayReport(string dayReportId)
        {
            try
            {
                _dayReport = _dayReportData.CreateDayReport(dayReportId);
                OnCurentDayReportSelect();
                OnDayReportDataChanged();
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
        }

        public void LoadDayReport(string dayReportID)
        {
            try
            {
               _dayReport = _dayReportData.LoadDayReport(dayReportID);
                OnCurentDayReportSelect();
                OnDayReportDataChanged();
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
        }

        public void DeleteDayReport(string dayReportId)
        {
            try
            {
                _dayReportData.DeleteDayReport(dayReportId);
                _dayReport = null;
                OnCurrentDayReportUnselect();
                new MessageBoxLogger().Log($"The day report: {dayReportId} was deleted from data base"
                                            , Logger.LogLevel.Information);

            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        public void UpdateInvoice(Invoice invoice)
        {
            try
            {
                if (_dayReport != null)
                {
                    _dayReportData.UpdateInvoice(invoice);
                    OnDayReportDataChanged();
                }
                else
                {
                    throw new ArgumentNullException("Day report is not selected");
                }
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
        }

        public bool CheckIsUnpaidInvoice(string invoiceId)
        {
            return _dayReportData.CheckIsUnpaidInvoice(invoiceId);
        }

        public bool CheckIsNewInvoice(string invoiceId)
        {
            return _dayReportData.CheckIsNewInvoice(invoiceId);
        }

        public void AddInvoice(Invoice invoice)
        {
            try
            {
                if (_dayReport != null)
                {
                    _dayReportData.AddNewInvoice(invoice);
                    _soundService.PlaySound(SoundEfect.AddInvoice);
                    OnDayReportDataChanged();
                }
                else
                {
                    throw new ArgumentNullException("There is not day report name set");
                }
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
        }

        public void RemoveInvoice(string invoiceId, int id)
        {
            try
            {
                if (_dayReport != null)
                {
                    _dayReportData.RemoveInvoice(invoiceId, id);
                    _soundService.PlaySound(SoundEfect.DeleteInvoice);
                    OnDayReportDataChanged();
                }
                else
                {
                    throw new ArgumentNullException("Day report is not selected");
                }
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
        }

        public bool IsValidLicencePlate(string licencePlate)
        {
            return _dayReportData.IsValidLicencePlate(licencePlate);
        }

        public void AddVehicle(string vehicle)
        {
            try
            {
                _dayReportData.AddVehicle(vehicle);
                OnVehiclesChanged();
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex.Message, Logger.LogLevel.Information);
            }
        }

        public void SetTransmissionDate(string date)
        {
            try
            {
                _dayReportData.SetTransmissionDate(date);
                OnTransmisionDateChanged();
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
        }

        public void AddMoney(decimal value, int count)
        {
            try
            {
                _dayReport.AddMoney(value, count);
                MoneyChanged();
                OnDayReportDataChanged();
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex.Message, Logger.LogLevel.Information);
            }
        }

        public void RemoveMoney(decimal value, int count)
        {
            try
            {
                _dayReport.RemoveMoney(value, count);
                MoneyChanged();
                OnDayReportDataChanged();
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex.Message, Logger.LogLevel.Information);
            }
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            try
            {
                if (_dayReport != null)
                {
                    return _dayReport.GetAllInvoices();
                }
                else
                {
                    throw new ArgumentNullException("There is not day report name set");
                }
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
                return Enumerable.Empty<Invoice>();
            }
        }

        public IEnumerable<string> GetAllVehicles()
        {
            List<string> vehicles = new List<string>();
            foreach (var vehicle in _dayReportData.Vehicles)
            {
                vehicles.Add(vehicle.ToString());
            }
            return vehicles.Order();
        }

        public IEnumerable<string> GetAllDayReportsID()
        {
            return _dayReportData.DayReportsId.Order();
        }

        public IDictionary<decimal, int> GetAllBanknotes()
        {
            return _dayReport.GetAllBanknotes();
        }

        private string DateConverter(string date)
        {
            return DateTime.Parse(date).ToString("yyyy-MM-dd");
        }

        private void OnDayReportsIdChanged()
        {
            DayReportsIdChanged?.Invoke();
        }

        private async Task RiseEventWhenExportCompleted(Task task)
        {
            await task;
            ExportCompleted?.Invoke();
            ExportFileCreate?.Invoke(_dayReportBuilder.ExportedFilePath);
        }

        private void OnDayReportDataChanged()
        {
            DayReportDataChanged.Invoke();
        }

        private void OnCurentDayReportSelect()
        {
            CurentDayReportSelect?.Invoke();
        }

        private void OnCurrentDayReportUnselect()
        {
            CurrentDayReportUnselected?.Invoke();
        }

        private void OnVehiclesChanged()
        {
            VehiclesChanged?.Invoke();
        }

        private void OnTotalsChanged()
        {
            TotalsChanged?.Invoke();
        }

        private void PlayMoneyChangeSound()
        {
            try 
            {
                _soundService.PlaySound(SoundEfect.Cash);
            }
            catch(ArgumentException ex) 
            {
                new FileLogger().Log(ex,Logger.LogLevel.Error);
            }
        }

        private void OnTransmisionDateChanged()
        {
            TransmisionDateChange?.Invoke();
        }

        private void SetTotalsToDayReportVievModel()
        {
            TotalAmount = _dayReport.TotalAmaunt;
            TotalIncome = _dayReport.TotalIncome;
            TotalExpenses = _dayReport.TotalExpenses;
            TotalNonPay = _dayReport.TotalNonPay;
            TotalOldInvoice = _dayReport.TotalOldInvoice;
            TotalWeight = _dayReport.TotalWeight;
            OnTotalsChanged();
        }

        private void ResetTotals()
        {
            TotalAmount = 0;
            TotalIncome = 0;
            TotalExpenses = 0;
            TotalNonPay = 0;
            TotalOldInvoice = 0;
            TotalWeight = 0;
            OnTotalsChanged();
        }

        private void SetTransmissionDateToDayReportViewModel()
        {
            TransmissionDate = _dayReport.TransmissionDate;
        }
    }
}
