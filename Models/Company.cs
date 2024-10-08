using DelitaTrade.Models.Interfaces.DataBase;
using DelitaTrade.Models.Interfaces.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocol;

namespace DelitaTrade.Models
{
    public class Company : ICompany, ISearchParametr, IDBData
    {
        private const int _numberOfReferences = 0;
        private readonly string _name;
        private string _type;
        private string _bulstad;
        private int _objectsCount;
        private List<CompanyObject> _objects;

        public Company(string name, string type, string bulstad)
        {
            _name = name;
            _type = type;
            _bulstad = bulstad;
            _objects = new List<CompanyObject>();
        }

        public event Action ObjectsDataBaseChange;

        public string FullName => $"{Name} {Type}";
        public string Name => _name;
        public string Type => _type;
        public string Bulstad => _bulstad;
        public int ObjectsCount => _objects.Count;

        public string Parameters => "company_name-=-company_type-=-company_bulstad";
        public string Data => $"{Name}-=-{Type}-=-{Bulstad}";
        public string Procedure => "add_company_full";

        public string SearchParametr => FullName;

        public int NumberOfReferences => _numberOfReferences;

        public bool TryAddNewObject(CompanyObject newCompanyObject)
        {
            if (_objects.FirstOrDefault(o => o.Name == newCompanyObject.Name) == null)
            { 
                _objects.Add(newCompanyObject);
                _objectsCount++;
                UpdateObjectsDataBase();
                return true;
            }
            return false;
        }

        public bool TryDeleteObject(CompanyObject companyObject)
        {
            CompanyObject objectToDelete = _objects.FirstOrDefault(o => o.Name == companyObject.Name);

            if (objectToDelete != null)
            {
                _objects.Remove(objectToDelete);
                _objectsCount--;
                UpdateObjectsDataBase();
                return true;
            }
            return false;
        }

        public bool UpdateCompanyObject(CompanyObject companyObject)
        {
            var obj = _objects.FirstOrDefault(o => o.Name == companyObject.Name);
            if (obj != null)
            {
                obj.UpdateCompanyObject(companyObject);
                return true;
            }
            return false;
        }

        public void UpdateCompanyData(Company company)
        { 
            _bulstad = company.Bulstad;
            _type = company.Type;            
        }

        public IEnumerable<CompanyObject> GetAllCompanyObjects()
        { 
            return _objects;
        }

        public SearchMethod GetSearchMethod()
        {
            return SearchMethod.CompanyName;
        }

        private void UpdateObjectsDataBase()
        {
            ObjectsDataBaseChange?.Invoke();
        }
    }
}
