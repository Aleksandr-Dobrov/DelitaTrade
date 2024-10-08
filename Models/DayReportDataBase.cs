using DelitaTrade.Models.Interfaces.DayReports;
using DelitaTrade.Models.Loggers;
using DelitaTrade.Models.DataProviders;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;

namespace DelitaTrade.Models
{
    public class DayReportDataBase : IDayReportIdDataBese
    {
        private const int _exor = 156;
        private const string _safeDataBasePath = "../../../DayReportsDataBase";
        private const string _invoicesIdFilePath = $"{_safeDataBasePath}/invoiceIdDataBase.dsc";
        private const string _dayReportIdFilePath = $"{_safeDataBasePath}/dayReportIdDataBase.dsc";
        private const string _vehiclesFilePath = $"{_safeDataBasePath}/vehiclesDataBase.dsc";

        [DataMember]
        private DayReport _dayReport;
        private DelitaTradeDayReport _delitaTradeDayReport;

        private IDelitaDataBase<DayReport> _dayReportDataProvider;

        private List<string> _dayReportsID;
        private Dictionary<string, List<string>> _invoicesId;
        private List<string> _vehicles;

        public DayReportDataBase(IDelitaDataBase<DayReport> dataBase, DelitaTradeDayReport delitaTradeDayReport)
        {
            _dayReportDataProvider = dataBase;            
            DayReportIdAdd += (string day) => { };
            DayReportIdRemove += (string day) => { };
            DayReportIdsLoad += (List<string> days) => { };
            DayReportsIdChanged += () => { };
            TryCreateSafeDirectory();
            TryLoadDayReportData();
            TryLoadInvoiceIdData();
            TryLoadListStringsData(ref _vehicles, _vehiclesFilePath);
            _delitaTradeDayReport = delitaTradeDayReport;
            _delitaTradeDayReport.CurentDayReportSelect += OndayReportEnable;
            _delitaTradeDayReport.CurrentDayReportUnselected -= OndayReportDesable;
        }

        public event Action DayReportsIdChanged;
        public event Action<List<string>> DayReportIdsLoad;
        public event Action<string> DayReportIdAdd;
        public event Action<string> DayReportIdRemove;

        public IEnumerable<string> DayReportsId => _dayReportsID;

        public IEnumerable<string> Vehicles => _vehicles;

        public DayReport CreateDayReport(string dayReportID)
        {
            try
            {
                if (IsNewDayReport(dayReportID))
                {
                    _dayReport = new DayReport(dayReportID);
                    CreateDayReportInDataBase(_dayReport);
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
            if (_dayReportsID.Contains(dayReportID))
            {
                LoadDayReportFromDataBase(dayReportID);
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
            _dayReportDataProvider.Path = SetDayReportFilePath(_dayReport.DayReportID);
            return _dayReportDataProvider.LoadAllData();
        }

        public void DeleteDayReport(string dayReportId)
        {
            if (IsNewDayReport(dayReportId) == false)
            {
                _dayReportsID.Remove(dayReportId);
                Dictionary<string, List<string>> invoiceToDelete = _invoicesId
                    .Where(i => i.Value
                        .All(r => r
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)[0] == dayReportId))
                    .ToDictionary();
                foreach (var item in invoiceToDelete)
                {
                    foreach (var invoice in item.Value)
                    {
                        string[] dayReportPairId = invoice.Split(",", StringSplitOptions.RemoveEmptyEntries);
                        RemoveInvoiceIdFromDayReport(dayReportId, item.Key, int.Parse(dayReportPairId[1]));
                    }
                }

                File.Delete(SetDayReportFilePath(dayReportId));

                SaveAllInvoiceIdToDataBase();
                SaveAllReportIdToDataBase();
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
                _dayReport.AddInvoice(invoice);
                SaveDayReportToDataBase(_dayReport);
                SaveInvoiceIdToDataBase(invoice.InvoiceID, _dayReport.DayReportID, invoice.Id);                
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
                _dayReport.RemoveInvoice(invoice, id);
                SaveDayReportToDataBase(_dayReport);
                RemoveInvoiceIdFromDayReport(_dayReport.DayReportID, invoice, id);
                SaveAllInvoiceIdToDataBase();
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
                SaveDayReportToDataBase(_dayReport);
            }
            else
            {
                throw new ArgumentNullException("The invoice is not exists!");
            }
        }

        public void AddVehicle(string vehicle)
        {
            if (IsValidLicencePlate(vehicle))
            {
                if (_dayReport != null && _dayReport.Vehicle != vehicle)
                {
                    MessageBoxResult result = MessageBox.Show("Add vehicle to day report?", 
                             "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if(result == MessageBoxResult.Yes)
                    {
                        _dayReport.Vehicle = vehicle;
                        SaveDayReportToDataBase(_dayReport);
                    }
                }
                if (_vehicles.Contains(vehicle.ToUpper()) == false)
                {
                    _vehicles.Add(vehicle.ToUpper());
                    SaveAllListStringsToDataBase(ref _vehicles, _vehiclesFilePath);
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
                        SaveDayReportToDataBase(_dayReport);
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
            _delitaTradeDayReport.MoneyChanged += SaveChangedDayReportToDataBase;
        }

        private void OndayReportDesable()
        {
            _delitaTradeDayReport.MoneyChanged -= SaveChangedDayReportToDataBase;
        }

        private bool IsNewInvoice(string invoiceId)
        {
            return _invoicesId.ContainsKey(invoiceId) == false;
        }

        private bool IsUnpaidInvoice(string invoiceId)
        {
            List<string> dayReportId;
            if (_invoicesId.TryGetValue(invoiceId, out dayReportId))
            {
                decimal amount = 0;
                decimal totalIncome = 0;
                string payMetod = string.Empty;

                for (int i = (dayReportId.Count) - 1; i >= 0; i--)
                {
                    string[] invoices = dayReportId[i].Split(",", StringSplitOptions.RemoveEmptyEntries);
                    
                    _dayReportDataProvider.Path = SetDayReportFilePath(invoices[0]);                    
                    var dayReport = _dayReportDataProvider.LoadAllData();                    
                    var invoice = dayReport.GetAllInvoices().First(i => i.InvoiceID == invoiceId && i.Id == int.Parse(invoices[1]));

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

        private decimal GetTotalIncomeFromInvoice(string invoiceId)
        {
            List<string> dayReportId;
            if (_invoicesId.TryGetValue(invoiceId, out dayReportId))
            {
                decimal amount = 0;
                decimal totalIncome = 0;
                foreach (var day in dayReportId)
                {
                    _dayReportDataProvider.Path = SetDayReportFilePath(dayReportId[^1]);
                    var dayReport = _dayReportDataProvider.LoadAllData();
                    var invoice = dayReport.GetAllInvoices().First(i => i.InvoiceID == invoiceId);
                    totalIncome += invoice.Income;
                    if (invoice.Amount > amount)
                    {
                        amount = invoice.Amount;
                    }
                }
                return totalIncome;
            }
            throw new InvalidOperationException("Invoice not found");
        }

        private string SetDayReportFilePath(string fileName)
        {
            string[] folders = fileName.Split('-');
            string filePath = string.Empty;
            if (folders.Length == 3)
            {
                string year = folders[0];
                string month = folders[1];
                filePath = $"{_safeDataBasePath}/{year}/{month}";
            }
            else
            {
                throw new ArgumentException("Incorrect report ID format! Format must be: [DD-MM-YYYY] ");
            }

            if (Directory.Exists(filePath) == false)
            {
                Directory.CreateDirectory(filePath);
            }

            return $"{filePath}/{fileName}.xml";
        }

        private void TryCreateSafeDirectory()
        {

            if (Directory.Exists(_safeDataBasePath) == false)
            {
                Directory.CreateDirectory(_safeDataBasePath);
            }
        }

        private void TryLoadListStringsData(ref List<string> values, string filePath)
        {
            if (File.Exists(filePath))
            {
                values = new List<string>();
                DataFileEncryptProvider.EncryptDataBaseFile(filePath, _exor);
                values = File.ReadAllLines(filePath).ToList();
                DataFileEncryptProvider.EncryptDataBaseFile(filePath, _exor);
            }
            else
            {
                values = new List<string>();
            }
        }

        private void TryLoadDayReportData()
        {
            if (File.Exists(_dayReportIdFilePath))
            {
                _dayReportsID = new List<string>();
                DataFileEncryptProvider.EncryptDataBaseFile(_dayReportIdFilePath, _exor);
                _dayReportsID = File.ReadAllLines(_dayReportIdFilePath).ToList();
                DataFileEncryptProvider.EncryptDataBaseFile(_dayReportIdFilePath, _exor);
            }
            else
            {
                _dayReportsID = new List<string>();
            }
            OnDayReportsIdChanged();
            OnDayReportIdsLoadComplete(_dayReportsID);
        }

        private void TryLoadInvoiceIdData()
        {
            if (File.Exists(_invoicesIdFilePath))
            {
                _invoicesId = new Dictionary<string, List<string>>();
                DataFileEncryptProvider.EncryptDataBaseFile(_invoicesIdFilePath, _exor);
                string[] loadData = File.ReadAllLines(_invoicesIdFilePath);
                DataFileEncryptProvider.EncryptDataBaseFile(_invoicesIdFilePath, _exor);
                foreach (string line in loadData)
                {
                    string[] invoiceIdDayReportIds = line.Split('=');
                    if (_invoicesId.ContainsKey(invoiceIdDayReportIds[0]))
                    {
                        _invoicesId[invoiceIdDayReportIds[0]].Add(invoiceIdDayReportIds[1]);
                    }
                    else
                    {
                        List<string> values = [invoiceIdDayReportIds[1]];
                        _invoicesId.Add(invoiceIdDayReportIds[0], values);
                    }                    
                }
            }
            else
            {
                _invoicesId = new Dictionary<string, List<string>>();
            }
        }
        
        private void SaveInvoiceIdToDataBase(string invoiceId, string dayReportId, int id)
        {
            if (_invoicesId.ContainsKey(invoiceId))
            {
                _invoicesId[invoiceId].Add($"{dayReportId},{id}");
            }
            else
            {
                List<string> values = [$"{dayReportId},{id}"];
                _invoicesId.Add(invoiceId, values);
            }
            DataFileEncryptProvider.EncryptDataBaseFile(_invoicesIdFilePath, _exor);
            using (StreamWriter safeInvoiceId = new StreamWriter(_invoicesIdFilePath, true))
            {
                safeInvoiceId.WriteLine($"{invoiceId}={dayReportId},{id}");
            }
            DataFileEncryptProvider.EncryptDataBaseFile(_invoicesIdFilePath, _exor);
        }

        private void SaveAllInvoiceIdToDataBase()
        {
            DataFileEncryptProvider.EncryptDataBaseFile(_invoicesIdFilePath, _exor);
            using (StreamWriter safeInvoiceId = new StreamWriter(_invoicesIdFilePath))
            {
                foreach (var invoice in _invoicesId)
                {
                    foreach (var dayReportId in invoice.Value)
                    {
                        safeInvoiceId.WriteLine($"{invoice.Key}={dayReportId}");
                    }
                }
            }
            DataFileEncryptProvider.EncryptDataBaseFile(_invoicesIdFilePath, _exor);
        }

        private void SaveDayReportIdToDataBase(string dayReportId)
        {
            _dayReportsID.Add(dayReportId);
            DataFileEncryptProvider.EncryptDataBaseFile(_dayReportIdFilePath, _exor);
            using (StreamWriter saveDayReportId = new StreamWriter(_dayReportIdFilePath, true))
            {
                saveDayReportId.WriteLine($"{dayReportId}");
            }
            DataFileEncryptProvider.EncryptDataBaseFile(_dayReportIdFilePath, _exor);
            OnDayReportsIdChanged();
            OnDayReportIdAdd(dayReportId);
        }

        private void SaveAllReportIdToDataBase()
        {
            DataFileEncryptProvider.EncryptDataBaseFile(_dayReportIdFilePath, _exor);
            using (StreamWriter saveDayReportId = new StreamWriter(_dayReportIdFilePath))
            {
                foreach (var dayreportId in _dayReportsID)
                {
                    saveDayReportId.WriteLine($"{dayreportId}");
                }
            }
            DataFileEncryptProvider.EncryptDataBaseFile(_dayReportIdFilePath, _exor);
            OnDayReportsIdChanged();
        }

        private void SaveAllListStringsToDataBase(ref List<string> values, string filePath)
        {
            DataFileEncryptProvider.EncryptDataBaseFile(filePath, _exor);
            using (StreamWriter saveValues = new StreamWriter(filePath))
            {
                foreach (var value in values)
                {
                    saveValues.WriteLine($"{value}");
                }
            }
            DataFileEncryptProvider.EncryptDataBaseFile(filePath, _exor);
        }

        private void SaveDayReportToDataBase(DayReport dayReport)
        {
            _dayReportDataProvider.Path = SetDayReportFilePath(dayReport.DayReportID);
            _dayReportDataProvider.SaveAllData(dayReport);
        }

        private void SaveChangedDayReportToDataBase()
        {
            SaveDayReportToDataBase(_dayReport);
        }

        private bool IsNewDayReport(string dayReportId)
        {
            if (_dayReportsID.Contains(dayReportId) == false)
            {                               
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CreateDayReportInDataBase(DayReport dayReport)
        {
            SaveDayReportToDataBase(dayReport);
            SaveDayReportIdToDataBase(dayReport.DayReportID);
        }

        private void LoadDayReportFromDataBase(string dayReportId)
        {
            _dayReportDataProvider.Path = SetDayReportFilePath(dayReportId);
            _dayReport = _dayReportDataProvider.LoadAllData();
        }

        private void OnDayReportsIdChanged()
        {
            DayReportsIdChanged?.Invoke();
        }

        private void OnDayReportIdsLoadComplete(List<string> dayReportIds)
        {
            DayReportIdsLoad.Invoke(dayReportIds);
        }

        private void RemoveInvoiceIdFromDayReport(string dayReportId, string invoiceId, int id)
        {
            if (_invoicesId.ContainsKey(invoiceId) && _invoicesId[invoiceId].Contains($"{dayReportId},{id}"))
            {
                if (_invoicesId[invoiceId].Count > 1)
                {
                    _invoicesId[invoiceId].Remove($"{dayReportId},{id}");
                }
                else
                {
                    _invoicesId.Remove(invoiceId);
                }
            }
            else 
            {
                throw new InvalidOperationException("Invoice can`t be remove!");
            }
        }

        private void OnDayReportIdAdd(string dayReportId)
        {
            DayReportIdAdd.Invoke(dayReportId);
        }

        private void OnDayReportIdRemove(string dayReportId)
        {
            DayReportIdRemove.Invoke(dayReportId);
        }

        private bool IsValidLicencePlate(string vehicle)
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
