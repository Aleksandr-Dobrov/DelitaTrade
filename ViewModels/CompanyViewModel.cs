using DelitaTrade.Models;
using System.Collections.ObjectModel;

namespace DelitaTrade.ViewModels
{
    public class CompanyViewModel : ViewModelBase, IDisposable
    {
        private readonly Company _company;

        private ObservableCollection<CompanyObjectViewModel> _companyObjects;

        public string CompanyName => _company.Name;
        public string CompanyType => _company.Type;
        public string Bulstad => _company.Bulstad;
        public int ObjectsCount => _company.ObjectsCount;

        public IEnumerable<CompanyObjectViewModel> CompanyObjects => _companyObjects;

        public CompanyViewModel(Company company)
        {
            _company = company;
            _companyObjects = new ObservableCollection<CompanyObjectViewModel>();
            UpdateObjectsDataBase();
            OnEnable();
        }

        private void OnEnable()
        { 
            _company.ObjectsDataBaseChange += UpdateObjectsDataBase;
        }

        private void OnDisable() 
        {
            _company.ObjectsDataBaseChange -= UpdateObjectsDataBase;
        }

        private void UpdateObjectsDataBase()
        { 
            _companyObjects.Clear();
            foreach (CompanyObject companyObject in _company.GetAllCompanyObjects())
            {
                CompanyObjectViewModel companyObjectViewModel = new CompanyObjectViewModel(companyObject);
                _companyObjects.Add(companyObjectViewModel);
            }
        }

        public void Dispose()
        {
            OnDisable();
        }
    }
}
