using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.MySqlDataBase;

namespace DelitaTrade.Models
{

    public class CompaniesDataBase : IDBDataParse
    {
        private MySqlReadCommand[] _readCommands;  
        private List<Company> _companies;

        public MySqlReadCommand[] ReadCommands => _readCommands;

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
                AddNewCompanyObject(new CompanyObject(results[0] ,results[3], results[4], results[6], ParsMySqlBool(results[5])));
            } 
        }

        private bool ParsMySqlBool(string arg)
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
    }
}
