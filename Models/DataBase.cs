using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class DataBase
    {
        [DataMember]
        private List<Company> _companies;
               
        public DataBase()
        {
            _companies = new List<Company>();
        }
                
        public void TryAddNewCompany(Company newCompany)
        {
            if (_companies.FirstOrDefault(c => c.Name == newCompany.Name) == null)
            { 
                _companies.Add(newCompany);                
            }
        }

        public void TryDeleteCompany(Company company)
        { 
            Company companyToDelete = _companies.FirstOrDefault(c =>c.Name == company.Name);

            if (companyToDelete != null) 
            {
                _companies.Remove(companyToDelete);
            }
        }

        public void UpdateCompanyData(Company company)
        {
            var companyToUpdate = _companies.First(c => c.Name == company.Name);
            companyToUpdate.UpdateCompanyData(company);
        }

        public void AddNewCompanyObject(CompanyObject newCompanyObject, string company)
        {  
            _companies.First(x => x.Name == company).TryAddNewObject(newCompanyObject);
        }

        public void DeleteCompanyObject(CompanyObject ObjectToDelete, string company)
        { 
            _companies.First(c => c.Name == company).TryDeleteObject(ObjectToDelete);
        }

        public void UpdateCompanyObject(CompanyObject companyObject, string company)
        { 
            _companies.First(c => c.Name == company).UpdateCompanyObject(companyObject);
        }
        
        public IEnumerable<Company> GetAllCompanies()
        {
            return _companies.OrderBy(c => c.Name);
        }
    }
}
