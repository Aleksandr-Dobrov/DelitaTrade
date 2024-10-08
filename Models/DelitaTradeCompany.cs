using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models
{
    public class DelitaTradeCompany
    {
        private CompaniesDataBase _dataBase;
        private readonly IDBProvider _mySqlDataBase;

        public DelitaTradeCompany(string name, IDBProvider dataBase)
        {
            Name = name;
            _mySqlDataBase = dataBase;
        }

        public event Action DataBaseChanged;

        public string Name { get; }

        public void LoadData()
        {
            _dataBase = (CompaniesDataBase)_mySqlDataBase.LoadAllData();
        }

        public void UpdateLoadDataBase()
        {            
            UpdateDataBase();
        }

        public void CreateNewCompany(Company newCompany)
        {
            if (_dataBase.TryAddNewCompany(newCompany))
            {
                _mySqlDataBase.Execute(new MySqlDBDataWriter(), newCompany);
                UpdateDataBase();
            }
        }

        public void DeleteCompany(Company deletedCompany) 
        {
            if (_dataBase.TryDeleteCompany(deletedCompany))
            { 
                _mySqlDataBase.Execute(new MySqlDBDataDeleter(), deletedCompany);
                UpdateDataBase();
            }
        }

        public void UpdateCompanyData(Company company)
        {
            if (_dataBase.UpdateCompanyData(company))
            { 
                _mySqlDataBase.Execute(new MySqlDBDataUpdater(), company);
                UpdateDataBase();
            }
        }

        public void CreateNewCompanyObject(CompanyObject newCompanyObject)
        {
            if (_dataBase.AddNewCompanyObject(newCompanyObject))
            {
                _mySqlDataBase.Execute(new MySqlDBDataWriter(), newCompanyObject);
                UpdateDataBase();
            }
        }

        public void DeleteCompanyObject(CompanyObject objectToDelete)
        {
            if (_dataBase.DeleteCompanyObject(objectToDelete))
            {
                _mySqlDataBase.Execute(new MySqlDBDataDeleter(), objectToDelete);
                UpdateDataBase();
            }
        }

        public void UpdateCompanyObject(CompanyObject companyObject)
        {
            if (_dataBase.UpdateCompanyObject(companyObject))
            { 
                _mySqlDataBase.Execute(new MySqlDBDataUpdater(), companyObject);
                UpdateDataBase();
            }
        }
        public IEnumerable<Company> GetAllCmpanies()
        {
            return _dataBase.GetAllCompanies();
        }

        private void UpdateDataBase()
        {
            DataBaseChanged?.Invoke();                        
        }
    }
}
