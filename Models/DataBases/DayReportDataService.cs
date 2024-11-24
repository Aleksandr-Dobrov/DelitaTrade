using DelitaTrade.Models.Builder.DataBase;
using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models;
using MySql.Data.MySqlClient;
using DelitaTrade.Models.MySqlDataBase;
using System.DirectoryServices.ActiveDirectory;

namespace DelitaTrade.Models.DataBases
{
    public class DayReportDataService
    {
        const string _user = "Александр Добров";

        private Dictionary<Type, IDBDataParser> _dbColection;

        private IDBDataParser _dbVehicles;
        private IDBDataParser _dbDayReports;
        private IDBDataParser _dbPayDesks;
        private IDBDataParser _dbInvoicesInDayReports;
        private MySqlParameter _mySqlParametr;

        private readonly IDBProvider _dbProvider;

        public DayReportDataService(IDBProvider dbProvider)
        {
            _dbProvider = dbProvider;
            _mySqlParametr = new MySqlParameter("user_name", _user);
            _dbVehicles = new DbManyObjService(new VehicleBuilder());
            _dbDayReports = new DbManyObjService(new DBDayReportBuilder());
            _dbDayReports.DataBuilder.AddParameter(_mySqlParametr);
            _dbPayDesks = new DbManyObjService(new DBPayDeskBuilder());
            _dbPayDesks.DataBuilder.AddParameter(_mySqlParametr);
            _dbInvoicesInDayReports = new DbManyObjService(new DBInvoiceInDayReportBuilder());
            _dbInvoicesInDayReports.DataBuilder.AddParameter(_mySqlParametr);
            _dbColection = new()
            {
                [_dbVehicles.DataBuilder.GetDbType()] = _dbVehicles,
                [_dbDayReports.DataBuilder.GetDbType()] = _dbDayReports,
                [_dbPayDesks.DataBuilder.GetDbType()] = _dbPayDesks,
                [_dbInvoicesInDayReports.DataBuilder.GetDbType()] = _dbInvoicesInDayReports
            };
            _dbProvider.LoadAllData(ref _dbVehicles);
            _dbProvider.LoadAllData(ref _dbDayReports);
            _dbProvider.LoadAllData(ref _dbPayDesks);
            _dbProvider.LoadAllData(ref _dbInvoicesInDayReports);
        }

        public IDBDataParser DbVehicle => _dbVehicles;
        public IEnumerable<string> DayReportsId => _dbDayReports.Select(x => ((IDBDataId)x).DBDataId);

        public IEnumerable<IDBData> Invoices => _dbInvoicesInDayReports;

        public void LoadAllVehicle()
        {
            _dbProvider.LoadAllData(ref _dbVehicles);
        }

        public DayReport LoadDayReport(string dayReportId)
        {
            List<Invoice> invoices = new List<Invoice>();
            DayReport dayReport = (DayReport)_dbDayReports.GetObject(new DayReport(dayReportId));            
            dayReport.SetPayDesk((PayDesk)_dbPayDesks.GetObject(new PayDesk(dayReport.PayDeskId)));

            MySqlDbReadProvider readProvider = new MySqlDbReadProvider(new MySqlDBConnection());

            foreach (var invoice in readProvider.ReadData(MySqlReadCommand.AllInvoiceInCurrentDayReport, new MySqlParameter("day_report_user", _user), new MySqlParameter("day_report_date", dayReport.DayReportID)))
            {
               Invoice invoiceToAdd = (Invoice)_dbInvoicesInDayReports.GetObject(new Invoice(int.Parse(invoice)));
                if (invoiceToAdd != null)
                {
                    invoices.Add(invoiceToAdd);
                }
            }
            dayReport.InitiateInvoices([.. invoices]);
            return dayReport;
        }

        public void SaveNewDataToDb(IDBData dataToSave) 
        {
            if (ContainsType(dataToSave) == false)
            {
                throw new ArgumentException("Data base type not exists");
            }
            else if (_dbColection[dataToSave.GetType()].AddData(dataToSave))
            {
                _dbProvider.Execute(new MySqlDBDataWriter(), dataToSave);
            }
            else
            {
                throw new InvalidOperationException("Data allready exists in data base");
            }
        }

        public void SaveData(IDBData dataToSave)
        {
            _dbColection[dataToSave.GetType()].AddData(dataToSave);
        }
        
        public void UpdateDataInDB(IDBData dataToUpdate)
        {
            if (ContainsType(dataToUpdate))
            {
                _dbProvider.Execute(new MySqlDBDataUpdater(), dataToUpdate);
            }
            else
            {
                throw new ArgumentException("Data base type not exists");
            }
        }

        public void DeleteDataFromDB(IDBData dataToDelete)
        {
            if (ContainsType(dataToDelete))
            {
                _dbProvider.Execute(new MySqlDBDataDeleter(), dataToDelete);
                DeleteFromColection(dataToDelete);
            }
            else
            {
                throw new ArgumentException("Data base type not exists");
            }
        }

        public void DeleteData(IDBData dataToDelete) 
        {
            if (ContainsType(dataToDelete))
            {
               DeleteFromColection(dataToDelete);
            }
            else
            {
                throw new ArgumentException("Data base type not exists");
            }
        }

        public IDBData GetDBObject(IDBData obj)
        {
            return _dbColection[obj.GetType()].GetObject(obj);
        }

        public bool ContainceKey(IDBData obj)
        {
            return _dbColection[obj.GetType()].ContainsKey(obj);
        }

        private void DeleteFromColection(IDBData dataToDelete) 
        {
            if (_dbColection[dataToDelete.GetType()].RemoveData(dataToDelete) == false)
            {
                throw new InvalidOperationException($"Can't delete invoice{dataToDelete.Data}");
            }
        }

        private bool ContainsType(IDBData data)
        {
            return _dbColection.ContainsKey(data.GetType());
        }

        public int GetNextInvoiceId()
        {
            if (_dbColection[typeof(Invoice)].Count == 0)
            {
                return 1;
            }
            var invoice = (Invoice)_dbColection[typeof(Invoice)].Last();
            int newId = invoice.Id + 1;
            while (_dbColection[typeof(Invoice)].ContainsKey(new Invoice(newId)))
            {
                newId++;
            }
            return newId;
        }

        public bool IsNewInvoice(string invoiceId)
        {
            MySqlDbReadProvider readProvider = new MySqlDbReadProvider(new MySqlDBConnection());
            return bool.Parse(readProvider.ReadData(MySqlReadCommand.IsNewInvoice, new MySqlParameter("invoice_id", invoiceId))[0]);
        }

        public IEnumerable<Invoice> GetInvoices(string invoiceId)
        {
            List<Invoice> invoices = new List<Invoice>();
            MySqlDbReadProvider readProvider = new MySqlDbReadProvider(new MySqlDBConnection());
            string[] invoicesid = readProvider.ReadData(MySqlReadCommand.GetIdByInvoiceId, new MySqlParameter("invoice_id", invoiceId));
            foreach (string id in invoicesid)
            {
                invoices.Add((Invoice)_dbInvoicesInDayReports.GetObject(new Invoice(int.Parse(id))));
            }
            return invoices;
        }
    }
}
