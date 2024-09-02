using DelitaTrade.Commands;
using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models;
using DelitaTradeProject.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;


namespace DelitaTrade.ViewModels
{
    public class AddNewCompanyViewModel : ViewModelBase
    {        
        private const string _initialCompanyType = "ООД";
        private const string _initialBulstad = "BG";
        private const string _initialAddrres = "";
        private string _companyType = _initialCompanyType;
        private string _bulstad = _initialBulstad;       
        private string _address = _initialAddrres;        
        private bool _bankPay;
        private string _lastCompanyName;
        
        private DelitaTradeCompany _delitaTrade;
       
        private ObservableCollection<CompanyViewModel> _companies;        

        private SearchBoxViewModel _searchBox;
        
        private SearchBoxObjectViewModel _searchBoxObject;

        private CompanyViewModel _currentCompany;

        private CompanyObjectViewModel _currentObject;

        private SearchBoxTextNotUpperDeletableItemViewModel _trader;
        private ObservableCollection<string> _tradersViewModel;
        private StringListDataBase _tradersModel;

        public IEnumerable<CompanyViewModel> Companies => _companies;
        
        public SearchBoxViewModel SearchBox => _searchBox; 

        public SearchBoxObjectViewModel SearchBoxObject => _searchBoxObject;

        public CompanyViewModel CurrentCompany => _currentCompany;

        public CompanyObjectViewModel CurrentObject => _currentObject;
        public SearchBoxTextNotUpperDeletableItemViewModel Trader => _trader;

        public event Action CompanySelected; 
        
        public event Action CompanyUnselected;

        public event Action ObjectSelected;

        public event Action ObjectUnselected;

        public AddNewCompanyViewModel(DelitaTradeCompany delitaTrade)
        {
            _delitaTrade = delitaTrade;
            _tradersViewModel = new ObservableCollection<string>();
            _tradersModel = new StringListDataBase("../../../SafeDataBase/Traders/traders.txt", "Trader");
            _trader = new SearchBoxTextNotUpperDeletableItemViewModel(_tradersViewModel, "Trader", _tradersModel);
            CreateCompanyCommand = new CreateCompanyCommand(this, delitaTrade);
            CreateObjectCommand = new CreateObjectCommand(this, delitaTrade);
            DeleteCompanyCommand = new DeleteCompanyCommand(this, delitaTrade);
            DeleteObjectCommand = new DeleteObjectCommand(this, delitaTrade);
            UpdateCompanyCommand = new UpdateCompanyCommand(this, delitaTrade);
            UpdateObjectCommand = new UpdateObjectCommand(this, delitaTrade);
            _companies = new ObservableCollection<CompanyViewModel>();
            _searchBox = new SearchBoxViewModel(_companies);            
            _searchBoxObject = new SearchBoxObjectViewModel(this);
            OnEnable();
        }

        private void OnEnable()
        {
            _searchBox.CompanyNameChanged += SelectCurrentCompany;
            _searchBoxObject.ObjectNameCanged += OnObjectNameChanged;
            _searchBoxObject.ObjectNameCanged += SelectCurrentObject;
            _delitaTrade.DataBaseChanged += UpdateCompanyDataBase;
            _delitaTrade.DataBaseChanged += SelectCurrentCompany;
            CompanyUnselected += RestoreCompanyinputData;
            CompanySelected += LoadCompanyInputData;
            ObjectSelected += LoadObjectInputData;
            ObjectUnselected += RestoreObjectInputData;
            _tradersModel.ColectionChainge += TradersDataUpdate;
            TradersDataUpdate();
            _trader.PropertyChanged += OnViewModelPropertyChanged;
        }
        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SearchBoxTextNotUpperDeletableItemViewModel.Item))
            {
                string item = (sender as SearchBoxTextNotUpperDeletableItemViewModel).Item;
                if (item != null && item != string.Empty && _tradersModel.Contains(item) == false)
                {
                    _tradersModel.Add(item);
                }
            }

        }

        private void TradersDataUpdate()
        {
            _tradersViewModel.Clear();
            foreach (var trader in _tradersModel)
            {
                _tradersViewModel.Add(trader);
            }
        }

        private void UpdateCompanyDataBase()
        {
            _lastCompanyName = CompanyName;            

            foreach (var company in _companies)
            {
                company.Dispose();
            }   
            _companies.Clear();
            foreach (Company company in _delitaTrade.GetAllCmpanies())
            {
                CompanyViewModel companyViewModel = new CompanyViewModel(company);
                _companies.Add(companyViewModel);
            }

            CompanyName = _lastCompanyName;
        }

        private void SelectCurrentCompany()
        {
            var currentCompany = _companies.FirstOrDefault(c => c.CompanyName == CompanyName);
            if (currentCompany != null)
            {
                _currentCompany = currentCompany;
                CompanySelected.Invoke();
            }
            else 
            {
                _currentCompany = null;
                CompanyUnselected.Invoke();
            }

            OnPropertyChange(nameof(CurrentCompany));
        }

        private void SelectCurrentObject()
        {
            var currentObject = _currentCompany?.CompanyObjects.FirstOrDefault(o => o.ObjectName == ObjectName);
            if (currentObject != null)
            {
                _currentObject = currentObject;
                ObjectSelected.Invoke();
            }
            else
            {
                _currentObject = null;
                ObjectUnselected.Invoke();
            }
        }

        private void OnObjectNameChanged()
        { 
            OnPropertyChange(nameof(ObjectName));
        }

        private void RestoreCompanyinputData()
        {
            CompanyType = _initialCompanyType;
            Bulstad = _initialBulstad;
        }

        private void LoadCompanyInputData()
        {
            CompanyType = _currentCompany.CompanyType;
            Bulstad = _currentCompany.Bulstad;
        }

        private void RestoreObjectInputData()
        { 
            Address = _initialAddrres;
            Trader.Item = _initialAddrres;
            BankPay = default;
        }

        private void LoadObjectInputData()
        { 
            Address = _currentObject.Adrress;
            Trader.Item = _currentObject.Trader;
            BankPay = _currentObject.BankPay;
        }

        public void SetBankPay(string IsBank)
        {
            if (IsBank == "Банка")
            {
                _bankPay = true;
            }
            else
            {
                _bankPay = false;
            }
        }

        public void SetObjectName(string name)
        {
            ObjectName = name;
        }
       
        public string CompanyName
        {
            get => _searchBox.InputText;
            set 
            {
                _searchBox.InputText = value;               
                OnPropertyChange();
            }
        }

        public string CompanyType
        {
            get => _companyType;
            set
            {   
                _companyType = value.ToUpper();
                OnPropertyChange();
            }
        }

        public string Bulstad
        {
            get => _bulstad;
            set 
            {
                _bulstad = value;
                OnPropertyChange();
            }
        }

        public string ObjectName
        {
            get => _searchBoxObject.InputTextObject;
            set
            {
                _searchBoxObject.InputTextObject = value;
                OnPropertyChange();
            }
        }

        public string Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChange();
            }
        }        

        public bool BankPay
        {
            get => _bankPay;
            set
            {
                _bankPay = value;
                OnPropertyChange();
            }
        }

        public ICommand CreateCompanyCommand { get; }
        public ICommand CreateObjectCommand { get; }
        public ICommand UpdateCompanyCommand { get; }
        public ICommand UpdateObjectCommand { get; }
        public ICommand DeleteCompanyCommand { get; }
        public ICommand DeleteObjectCommand { get; }
    }
}
