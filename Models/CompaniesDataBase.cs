using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;
using MySql.Data.MySqlClient;
using System.Collections;

namespace DelitaTrade.Models
{

    public class CompaniesDataBase : IDBDataParser
    {
        private MySqlReadCommand[] _readCommands;  
        private List<Company> _companies;

        public MySqlReadCommand[] ReadCommands => _readCommands;

        public MySqlParameter[] Parameters => throw new NotImplementedException();

        public IDbDataBuilder DataBuilder => throw new NotImplementedException();

        public int Count => _companies.Count;

        public CompaniesDataBase()
        {
            _companies = new List<Company>();
            _readCommands = [MySqlReadCommand.AllObjects, MySqlReadCommand.AllCompanies];
        }
                
        public bool TryAddNewCompany(Company newCompany)
        {
            if (_companies.FirstOrDefault(c => c.Name == newCompany.Name) == null)
            { 
                _companies.Add(newCompany);
                return true;
            }
            return false;
        }

        public bool TryDeleteCompany(Company company)
        { 
            Company companyToDelete = _companies.FirstOrDefault(c =>c.Name == company.Name);

            if (companyToDelete != null) 
            {
                _companies.Remove(companyToDelete);
                return true;
            }
            return false;
        }

        public bool UpdateCompanyData(Company company)
        {
            var companyToUpdate = _companies.FirstOrDefault(c => c.Name == company.Name);
            if (companyToUpdate != null)
            {
                companyToUpdate.UpdateCompanyData(company);
                return true;
            }
            return false;
        }

        public bool AddNewCompanyObject(CompanyObject newCompanyObject)
        {  
            return _companies.First(x => x.Name == newCompanyObject.CompanyName).TryAddNewObject(newCompanyObject);
        }

        public bool DeleteCompanyObject(CompanyObject ObjectToDelete)
        {
            var comp = _companies.FirstOrDefault(c => c.Name == ObjectToDelete.CompanyName);
            if (comp != null)
            {
                return comp.TryDeleteObject(ObjectToDelete);
            }
            return false;
        }

        public bool UpdateCompanyObject(CompanyObject companyObject)
        {
            var comp = _companies.FirstOrDefault(c => c.Name == companyObject.CompanyName);
            if (comp != null)
            {
                return comp.UpdateCompanyObject(companyObject);
            }
            return false;
        }
        
        public IEnumerable<Company> GetAllCompanies()
        {
            return _companies.OrderBy(c => c.Name);
        }

        public void Parse(string inputData)
        {
            string[] results = inputData.Split("-=-");
            TryAddNewCompany(new Company(results[0], results[1], results[2]));
            if (results.Length > 3 && results[3] != string.Empty)
            {
                AddNewCompanyObject(new CompanyObject(results[0] ,results[3], results[4], results[6], ParseMySqlBool(results[5])));
            } 
        }

        public IEnumerator<IDBData> GetEnumerator()
        {
            foreach (IDBData data in _companies)
            {
                yield return data;
            }            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _companies.GetEnumerator();
        }

        public bool ContainsKey(IDBData dBData)
        {
            return _companies.Contains(dBData);
        }

        public IDBData GetObject(IDBData data)
        {
            return _companies.FirstOrDefault(c => c == data);
        }

        private bool ParseMySqlBool(string arg)
        {
            if (arg == null || arg == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool AddData(IDBData data)
        {
            if (_companies.Contains(data))
            { 
                return false;
            }
            _companies.Add((Company)data);                
            return true;

        }

        public bool RemoveData(IDBData data)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<Company> companies => _companies;
    }
}
