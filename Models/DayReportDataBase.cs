using DelitaTrade.Models.Interfaces.DayReports;
using DelitaTrade.Models.Loggers;
using System.Windows;
using DelitaTrade.Models.DataBases;
using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.DataBases.DayReportDataBase;
using DelitaTrade.Models.MySqlDataBase;
using Microsoft.Extensions.Configuration;

namespace DelitaTrade.Models
{
    public class DayReportDataBase : IDayReportIdDataBese
    {
        private int _payDeskId = 0;

        private DayReport _dayReport;
        private DelitaTradeDayReport _delitaTradeDayReport;
        private DayReportDataService _dayReportDataService;

        private readonly IDBProvider _dbProvider;

        public DayReportDataBase(DelitaTradeDayReport delitaTradeDayReport, IDBProvider dBProvider, IConfiguration configuration)
        {
            _dbProvider = dBProvider;          
            DayReportIdAdd += (string day) => { };
            DayReportIdRemove += (string day) => { };
            DayReportIdsLoad += (List<string> days) => { };
            DayReportsIdChanged += () => { };
            _dayReportDataService = new DayReportDataService(dBProvider, configuration);
            _delitaTradeDayReport = delitaTradeDayReport;
            _delitaTradeDayReport.CurentDayReportSelect += OndayReportEnable;
            _delitaTradeDayReport.CurrentDayReportUnselected -= OndayReportDesable;
        }

        public event Action DayReportsIdChanged;
        public event Action<List<string>> DayReportIdsLoad;
        public event Action<string> DayReportIdAdd;
        public event Action<string> DayReportIdRemove;

        public IEnumerable<string> DayReportsId => _dayReportDataService.DayReportsId;

        public IDBDataParser Vehicles => _dayReportDataService.DbVehicle;

        public DayReport CreateDayReport(string dayReportID)
        {
            try
            {
                if (IsNewDayReport(new DayReport(dayReportID)))
                {
                    _dayReport = new DayReport(dayReportID, _payDeskId--);
                    _dayReportDataService.SaveNewDataToDb(_dayReport);
                    _dayReportDataService.SaveData(_dayReport.PayDesk);
                    OnDayReportsIdChanged();
                    OnDayReportIdAdd(dayReportID);
                    return _dayReport;
                }
                else
                {
                    throw new ArgumentException("The day report is already exists");
                }
            }
            catch (Exception ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Information);
                return _dayReport;
            }
        }

        public DayReport LoadDayReport(string dayReportID)
        {            
            if (IsNewDayReport(new DayReport(dayReportID)) == false)
            {
                _dayReport = _dayReportDataService.LoadDayReport(dayReportID);
                return _dayReport;
            }
            else
            {
                throw new ArgumentException("Day report is not exists in data base.");
            }
        }

        public DayReport LoadCopyDayReport()
        {
            if (_dayReport == null)
            {
                throw new ArgumentNullException("Day report is not loaded");
            }
            
            return (DayReport)_dayReport.Clone();
        }

        public void DeleteDayReport(string dayReportId)
        {
            DayReport key = new DayReport(dayReportId);
            if (IsNewDayReport(key) == false)
            { 
                DayReport dayReport = (DayReport)_dayReportDataService.GetDBObject(key);
                _dayReportDataService.DeleteData(dayReport.PayDesk);
                foreach (var invoice in dayReport.Invoices)
                {
                    _dayReportDataService.DeleteData(invoice);
                }
                _dayReportDataService.DeleteDataFromDB(dayReport);
                OnDayReportsIdChanged();
                OnDayReportIdRemove(dayReportId);
            }
            else
            {
                throw new ArgumentException($"Day report: {dayReportId} is not exists");
            }
        }

        public void AddNewInvoice(Invoice invoice) 
        {
            if (IsNewInvoice(invoice.InvoiceID) || IsUnpaidInvoice(invoice.InvoiceID))
            {
                invoice.SetId(_dayReportDataService);
                _dayReport.AddInvoice(invoice);
                _dayReportDataService.SaveNewDataToDb(invoice);
                _dayReportDataService.UpdateDataInDB(_dayReport);                
            }
            else
            {
                throw new ArgumentException("The invoice is already exists!");
            }
        }

        public void RemoveInvoice(string invoice, int id)
        {
            if (invoice != null && (IsNewInvoice(invoice) == false))
            {                
                _dayReportDataService.DeleteDataFromDB(_dayReport.RemoveInvoice(invoice, id));
                _dayReportDataService.UpdateDataInDB(_dayReport);
            }           
            else
            {
                throw new ArgumentNullException("The invoice is not exists!");
            }
        }

        public void UpdateInvoice(Invoice invoice)
        {
            if (invoice != null && (IsNewInvoice(invoice.InvoiceID) == false))
            {
                _dayReport.UpdateInvoice(invoice);
                _dayReportDataService.UpdateDataInDB(invoice);
                _dayReportDataService.UpdateDataInDB(_dayReport);
            }
            else
            {
                throw new ArgumentNullException("The invoice is not exists!");
            }
        }

        public void AddVehicle(string vehicle)
        {
            if (IsValidLicensePlate(vehicle))
            {
                if (_dayReport != null && _dayReport.Vehicle != vehicle)
                {                    
                    _dayReport.Vehicle = vehicle;
                    _dayReportDataService.UpdateDataInDB(_dayReport);     
                }
                if (_dayReportDataService.DbVehicle.ContainsKey(new Vehicle(vehicle, "nome")) == false)
                {
                    _dbProvider.Execute(new MySqlDBDataWriter(), new Vehicle(vehicle, "nome"));
                    _dayReportDataService.LoadAllVehicle();
                }
            }
            else 
            {
                throw new ArgumentException("Licence plate format is incorrect");
            }
        }

        public void SetTransmissionDate(string date)
        {
            if (_dayReport != null)
            {
                if (_dayReport.TransmissionDate != date)
                {
                    MessageBoxResult result = MessageBox.Show("Set date?",
                                 "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        _dayReport.TransmissionDate = date;
                        _dayReportDataService.UpdateDataInDB(_dayReport);
                    }                    
                }
            }
            else
            {
                throw new ArgumentNullException("Day report is not select");
            }
        }

        public bool CheckIsUnpaidInvoice(string invoiceId)
        {
            return IsUnpaidInvoice(invoiceId);
        }

        public bool CheckIsNewInvoice(string invoiceId)
        {
            return IsNewInvoice(invoiceId);
        }

        private void OndayReportEnable()
        {
            _delitaTradeDayReport.MoneyChanged += UpdateDayReportInDB;
        }

        private void OndayReportDesable()
        {
            _delitaTradeDayReport.MoneyChanged -= UpdateDayReportInDB;
        }

        private bool IsNewInvoice(string invoiceId)
        {
            if(_dayReportDataService.IsNewInvoice(invoiceId))
            {
                return true;
            }
            return false;
        }

        private bool IsUnpaidInvoice(string invoiceId)
        {
            decimal amount = 0;
            decimal totalIncome = 0;
            string payMetod = string.Empty;
            
            var result = _dayReportDataService.GetInvoices(invoiceId);

            if (result != null) 
            {
                foreach (Invoice invoice in result)
                {
                    totalIncome += invoice.Income;
                    if (invoice.Amount > amount)
                    {
                        amount = invoice.Amount;
                    }
                    payMetod = invoice.PayMethod;
                }
                                
                if ((payMetod == "В брой" || payMetod == "С карта" || payMetod == "Стара сметка") 
                    && amount > totalIncome)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            throw new ArgumentException("Invoice is not exists in databa");
        }

        private void UpdateDayReportInDB()
        {
            if (_dayReport != null)
            {
                _dayReportDataService.UpdateDataInDB(_dayReport);
            }
        }

        private bool IsNewDayReport(DayReport key)
        {
            if (_dayReportDataService.ContainceKey(key) == false)
            {                               
                return true;
            }
            else
            {
                return false;
            }
        }

        private void OnDayReportsIdChanged()
        {
            DayReportsIdChanged?.Invoke();
        }

        private void OnDayReportIdAdd(string dayReportId)
        {
            DayReportIdAdd.Invoke(dayReportId);
        }

        private void OnDayReportIdRemove(string dayReportId)
        {
            DayReportIdRemove.Invoke(dayReportId);
        }
                
        public bool IsValidLicensePlate(string vehicle)
        {
            if (vehicle.Length == 10)
            {
                return char.IsLetter(vehicle[0])
                && char.IsLetter(vehicle[1])
                && vehicle[2] == ' '
                && char.IsDigit(vehicle[3])
                && char.IsDigit(vehicle[4])
                && char.IsDigit(vehicle[5])
                && char.IsDigit(vehicle[6])
                && vehicle[7] == ' '
                && char.IsLetter(vehicle[8])
                && char.IsLetter(vehicle[9]);
            }
            else if (vehicle.Length == 9)
            {
                return char.IsLetter(vehicle[0])
                && vehicle[1] == ' '
                && char.IsDigit(vehicle[2])
                && char.IsDigit(vehicle[3])
                && char.IsDigit(vehicle[4])
                && char.IsDigit(vehicle[5])
                && vehicle[6] == ' '
                && char.IsLetter(vehicle[7])
                && char.IsLetter(vehicle[8]);
            }
            else
            {
                return false;
            }
        }
    }
}
