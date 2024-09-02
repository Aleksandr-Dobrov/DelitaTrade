using DelitaTrade.Models.Loggers;
using System.IO;
using System.Runtime.Serialization;
using System.Windows;
using static DelitaTrade.Models.Loggers.Logger;

namespace DelitaTrade.Models
{
    public class DelitaTradeCompany
    {
        [DataMember]
        private DataBase _dataBase;

        private readonly IDataBase<DataBase> _XmlDataBase;
       
        private string _filePath = "../../../SafeDataBase/DelitaTradeDataBase.xml";

        private const string _backupFilePath = "../../../SafeBackupDataBase/DelitaTradeBackupDataBase";

        public event Action DataBaseChanged;
        public string Name { get; }

        public DelitaTradeCompany(string name, IDataBase<DataBase> dataBase)
        {
            Name = name;
            _dataBase = new DataBase();           
            _XmlDataBase = dataBase;
            _XmlDataBase.UsedDefaultPath += UseDefaultPath;
            DataBaseChanged += SaveData;
        }
        private void UpdateDataBase()
        {
            DataBaseChanged?.Invoke();                        
        }

        private bool IsValidDataToSafe()
        {
            return ((File.Exists(GetBackupDataBasePath(DateTime.Now.Date)) == false)
                || ((_ = new FileInfo(GetBackupDataBasePath(DateTime.Now.Date))).Length
                <= (_ = new FileInfo(_filePath)).Length));
              
        }

        private void UseDefaultPath(string message)
        {            
            new FileLogger().Log($"Data base use {message} file path!",LogLevel.Information)
                            .Log($"Data base use {message} file path!", LogLevel.Information);
            _filePath = message;
        }

        private string GetBackupDataBasePath(DateTime date)
        {
            return $"{_backupFilePath}{date:dd-MM-yyyy}.xml";
        }

        private void SaveBacupData()
        {
            if (IsValidDataToSafe())
            {
                _XmlDataBase.Path = GetBackupDataBasePath(DateTime.Now.Date);
                _XmlDataBase.SaveAllData(_dataBase);
            }
            else
            {
                throw new InvalidOperationException("Can not save backup data base!");
            }
        }

        private void SaveData()
        {
            try
            {
                _XmlDataBase.Path = _filePath;
                _XmlDataBase.SaveAllData(_dataBase);
                SaveBacupData();                
            }
            catch (Exception ex) 
            {
                new FileLogger().Log(ex, LogLevel.Error);
            }
        }

        private DataBase LoadData(string filePath) 
        {
            try
            {
                _XmlDataBase.Path = filePath;
                return _XmlDataBase.LoadAllData();
            }
            catch 
            {
                return new DataBase();
            }
        }

        public void LoadFile()
        {
            _dataBase = LoadData(_filePath);
        }

        public void LoadFile(string filePath)
        {
            _dataBase = LoadData(filePath);
        }

        public void UpdateLoadDataBase()
        {            
            UpdateDataBase();
        }

        public void CreateNewCompany(Company newCompany)
        { 
            _dataBase.TryAddNewCompany(newCompany);
            UpdateDataBase();
        }

        public void DeleteCompany(Company newCompany) 
        {
            _dataBase.TryDeleteCompany(newCompany);
            UpdateDataBase();
        }

        public void UpdateCompanyData(Company company)
        {
            _dataBase.UpdateCompanyData(company);
            UpdateDataBase();
        }

        public void CreateNewCompanyObject(CompanyObject newCompanyObject, string company)
        {
            _dataBase.AddNewCompanyObject(newCompanyObject, company);
            UpdateDataBase();
        }

        public void DeleteCompanyObject(CompanyObject objectToDelete, string company)
        { 
            _dataBase.DeleteCompanyObject(objectToDelete, company);
            UpdateDataBase();
        }

        public void UpdateCompanyObject(CompanyObject companyObject, string company)
        { 
            _dataBase.UpdateCompanyObject(companyObject, company);
            UpdateDataBase();
        }
        public IEnumerable<Company> GetAllCmpanies()
        {
            return _dataBase.GetAllCompanies();
        }
    }
}
