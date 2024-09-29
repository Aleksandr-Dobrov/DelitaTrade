using DelitaTrade.Interfaces.ReturnProtocol;
using DelitaTrade.Models.ReturnProtocol;
using System.Runtime.Serialization;

namespace DelitaTrade.Models
{
    [DataContract]
    public class Company : ICompany, ISearchParametr
    {
        [DataMember]
        private readonly string _name;
        [DataMember]
        private string _type;
        [DataMember]
        private string _bulstad;
        [DataMember]
        private int _objectsCount;
        [DataMember]
        private List<CompanyObject> _objects;

        public event Action ObjectsDataBaseChange;
        public Company(string name, string type, string bulstad)
        {
            _name = name;
            _type = type;
            _bulstad = bulstad;
            _objects = new List<CompanyObject>();
        }
        public string SearchParametr => FullName;
        public string FullName => $"{Name} {Type}";
        public string Name { get => _name; }
        public string Type { get => _type; }
        public string Bulstad { get => _bulstad; }
        public int ObjectsCount { get => _objects.Count;}

        private void UpdateObjectsDataBase()
        {
            ObjectsDataBaseChange?.Invoke();
        }

        public void TryAddNewObject(CompanyObject newCompanyObject)
        {
            if (_objects.FirstOrDefault(o => o.Name == newCompanyObject.Name) == null)
            { 
                _objects.Add(newCompanyObject);
                _objectsCount++;
                UpdateObjectsDataBase();
            }
        }

        public void TryDeleteObject(CompanyObject companyObject)
        {
            CompanyObject objectToDelete = _objects.FirstOrDefault(o => o.Name == companyObject.Name);

            if (objectToDelete != null)
            {
                _objects.Remove(objectToDelete);
                _objectsCount--;
                UpdateObjectsDataBase();
            }
        }

        public void UpdateCompanyObject(CompanyObject companyObject)
        { 
            _objects.First(o => o.Name == companyObject.Name).UpdateCompanyObject(companyObject);
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
    }
}
