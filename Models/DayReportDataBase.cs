﻿using DelitaTrade.Interfaces.DayReport;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;

namespace DelitaTrade.Models
{
    public class DayReportDataBase : IDayReportIdDataBese
    {
        [DataMember]
        private DayReport _dayReport;

        private DelitaTradeDayReport _delitaTradeDayReport;

        private List<string> _dayReportsID;

        private Dictionary<string, string> _invoicesId;

        private List<string> _vehicles;

        private const string _safeDataBasePath = "../../../DayReportsDataBase";

        private const string _invoicesIdFilePath = $"{_safeDataBasePath}/invoiceIdDataBase.dsc";

        private const string _dayReportIdFilePath = $"{_safeDataBasePath}/dayReportIdDataBase.dsc";

        private const string _vehiclesFilePath = $"{_safeDataBasePath}/vehiclesDataBase.dsc";

        private IDataBase<DayReport> _dayReportDataProvider;

        public DayReportDataBase(IDataBase<DayReport> dataBase, DelitaTradeDayReport delitaTradeDayReport)
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

        private void EncryptDataBaseFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream encrypting = new FileStream(filePath, FileMode.Open))
                {
                    byte[] data = new byte[encrypting.Length];

                    encrypting.Read(data, 0, data.Length);

                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = (byte)(data[i] ^ 156);
                    }

                    encrypting.Seek(0, SeekOrigin.Begin);
                    encrypting.Write(data, 0, data.Length);
                }
            }
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
                EncryptDataBaseFile(filePath);
                values = File.ReadAllLines(filePath).ToList();
                EncryptDataBaseFile(filePath);
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
                EncryptDataBaseFile(_dayReportIdFilePath);
                _dayReportsID = File.ReadAllLines(_dayReportIdFilePath).ToList();
                EncryptDataBaseFile(_dayReportIdFilePath);
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
                _invoicesId = new Dictionary<string, string>();
                EncryptDataBaseFile(_invoicesIdFilePath);
                string[] loadData = File.ReadAllLines(_invoicesIdFilePath);
                EncryptDataBaseFile(_invoicesIdFilePath);
                foreach (string line in loadData)
                {
                    string[] split = line.Split('=');
                    _invoicesId.Add(split[0], split[1]);
                }
            }
            else
            {
                _invoicesId = new Dictionary<string, string>();
            }
        }
        
        private void SaveInvoiceIdToDataBase(string invoiceId, string dayReportId)
        {
            _invoicesId.Add(invoiceId, dayReportId);
            EncryptDataBaseFile(_invoicesIdFilePath);
            using (StreamWriter safeInvoiceId = new StreamWriter(_invoicesIdFilePath, true))
            {
                safeInvoiceId.WriteLine($"{invoiceId}={dayReportId}");
            }
            EncryptDataBaseFile(_invoicesIdFilePath);
        }

        private void SaveAllInvoiceIdToDataBase()
        {
            EncryptDataBaseFile(_invoicesIdFilePath);
            using (StreamWriter safeInvoiceId = new StreamWriter(_invoicesIdFilePath))
            {
                foreach (var invoice in _invoicesId)
                {
                    safeInvoiceId.WriteLine($"{invoice.Key}={invoice.Value}");
                }
            }
            EncryptDataBaseFile(_invoicesIdFilePath);
        }

        private void SaveDayReportIdToDataBase(string dayReportId)
        {
            _dayReportsID.Add(dayReportId);
            EncryptDataBaseFile(_dayReportIdFilePath);
            using (StreamWriter saveDayReportId = new StreamWriter(_dayReportIdFilePath, true))
            {
                saveDayReportId.WriteLine($"{dayReportId}");
            }
            EncryptDataBaseFile(_dayReportIdFilePath);
            OnDayReportsIdChanged();
            OnDayReportIdAdd(dayReportId);
        }

        private void SaveAllReportIdToDataBase()
        {
            EncryptDataBaseFile(_dayReportIdFilePath);
            using (StreamWriter saveDayReportId = new StreamWriter(_dayReportIdFilePath))
            {
                foreach (var dayreportId in _dayReportsID)
                {
                    saveDayReportId.WriteLine($"{dayreportId}");
                }
            }
            EncryptDataBaseFile(_dayReportIdFilePath);
            OnDayReportsIdChanged();
        }

        private void SaveAllListStringsToDataBase(ref List<string> values, string filePath)
        {
            EncryptDataBaseFile(filePath);
            using (StreamWriter saveValues = new StreamWriter(filePath))
            {
                foreach (var value in values)
                {
                    saveValues.WriteLine($"{value}");
                }
            }
            EncryptDataBaseFile(filePath);
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

        private void OnDayReportIdAdd(string dayReportId)
        {
            DayReportIdAdd.Invoke(dayReportId);
        }

        private void OnDayReportIdRemove(string dayReportId)
        {
            DayReportIdRemove.Invoke(dayReportId);
        }

        public event Action DayReportsIdChanged;

        public event Action<List<string>> DayReportIdsLoad;
        public event Action<string> DayReportIdAdd;
        public event Action<string> DayReportIdRemove;

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

        public void DeleteDayReport(string dayReportId)
        {
            if (IsNewDayReport(dayReportId) == false)
            {
                _dayReportsID.Remove(dayReportId);
                Dictionary<string, string> invoiceToDelete = _invoicesId.Where(i => i.Value == dayReportId).ToDictionary();
                foreach (var item in invoiceToDelete)
                {
                    _invoicesId.Remove(item.Key);
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
            if (IsNewInvoice(invoice.InvoiceID))
            {
                _dayReport.AddInvoice(invoice);
                SaveDayReportToDataBase(_dayReport);
                SaveInvoiceIdToDataBase(invoice.InvoiceID, _dayReport.DayReportID);
            }
            else
            {
                throw new ArgumentException("The invoice is already exists!");
            }
        }

        public void RemoveInvoice(string invoice)
        {
            if (invoice != null && (IsNewInvoice(invoice) == false))
            {
                _dayReport.RemoveInvoice(invoice);
                SaveDayReportToDataBase(_dayReport);
                _invoicesId.Remove(invoice);
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

        public IEnumerable<string> DayReportsId => _dayReportsID;

        public IEnumerable<string> Vehicles => _vehicles;
    }
}
