using DelitaTrade.Commands;
using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models;
using DelitaTrade.Models.DataProviders;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DelitaTrade.ViewModels
{
    public class DayReportsViewModel : ViewModelBase
    {
        private CurrencyProvider _currencyProvider;
        private AddNewCompanyViewModel _addNewCompanyViewModel;

        private PayMethodBoxViewModel _payMethodBoxViewModel;

        private LoadDayReportBoxViewModel _loadDayReportBoxViewModel;
        
        private DayReportTotalsViewModel _dayReportTotalsViewModel;

        private ObservableCollection<InvoiceViewModel> _invoices;
        private InvoiceViewModel _selectedInvoiceViewModel;
       
        private DelitaTradeDayReport _delitaTradeDayReport;

        private CurrentDayReportViewModel _currentDayReportViewModel;

        private ObservableCollection<string> _dayReporsId;

        private DayReportIdViewModel _dayReportIdViewModel;
                
        private DateTime _date = DateTime.Now.Date;        

        private string _addOrUpdateCommand = "Add";

        private double _weight;
        private decimal _income;
        private decimal _amount;
        private string _invoiceID;
        private string _previousInvoiceID = "1000000000";
        private string _dayReportColor = "Red";

        private bool _isPayMethodLoad = false;

        public DayReportsViewModel(DelitaTradeDayReport delitaTradeDayReport ,ViewModelBase addNewCompanyViewModel)
        {
            _currencyProvider = new CurrencyProvider();
            _delitaTradeDayReport = delitaTradeDayReport;
            _addNewCompanyViewModel = (AddNewCompanyViewModel?)addNewCompanyViewModel;
            _payMethodBoxViewModel = new PayMethodBoxViewModel();
            _dayReporsId = new ObservableCollection<string>();
            _currentDayReportViewModel = new CurrentDayReportViewModelNull();            
            _dayReportTotalsViewModel = new DayReportTotalsViewModel(delitaTradeDayReport);
            _invoices = new ObservableCollection<InvoiceViewModel>();
            AddOrUpdateInvoiceCommand = new AddOrUpdateInvoiceCommand(_delitaTradeDayReport, _addNewCompanyViewModel, this);
            _dayReportIdViewModel = new DayReportIdViewModel(delitaTradeDayReport.DayReportIdDataBese);
            CreateNewDayReportCommand = new CreateNewDayReportCommand(_delitaTradeDayReport, this);
            LoadDayReportCommand = new LoadDayReportCommand(_delitaTradeDayReport, this);
            DeleteDayReportCommand = new DeleteDayReportCommand(_delitaTradeDayReport, this);
            RemoveInvoiceCommand = new RemoveInvoiceCommand(_delitaTradeDayReport, this);           
            OnEnable();
        }

        public InvoiceViewModel SelectedInvoiceViewModel
        {
            get { return _selectedInvoiceViewModel; }
            set 
            { 
                _selectedInvoiceViewModel = value;
                OnPropertyChange();
            }
        }
        public SearchBoxViewModel SearchBox => _addNewCompanyViewModel.SearchBox;

        public SearchBoxObjectViewModel SearchBoxObject => _addNewCompanyViewModel.SearchBoxObject;

        public PayMethodBoxViewModel PayMethodBox => _payMethodBoxViewModel;

        public DayReportTotalsViewModel DayReportTotalsViewModel => _dayReportTotalsViewModel;

        public LoadDayReportBoxViewModel LoadDayReportsIdBox => _loadDayReportBoxViewModel;

        public DayReportIdViewModel DayReportIdViewModel => _dayReportIdViewModel;

        public CurrentDayReportViewModel CurrentDayReportViewModel => _currentDayReportViewModel;

        public string LoadDayReportId => _dayReportIdViewModel.DayReportId;

        public string DayReportId => _currentDayReportViewModel.DayReportId;

        public IEnumerable<string> DayReportsId => _dayReporsId;
                
        public IEnumerable<InvoiceViewModel> Invoices => _invoices;

        public event Action PaymentChange;

        public event Action LoadInvoice;
        
        public event Action AmountChange;

        private void OnEnable()
        {
            _addNewCompanyViewModel.PropertyChanged += OnAddCompanyPropertyChanged;
            _delitaTradeDayReport.DayReportDataChanged += UpdateDayReportData;
            _addNewCompanyViewModel.ObjectSelected += SetPayMethod;
            _payMethodBoxViewModel.PropertyChanged += PayMenthodBoxPropertyChanged;
            PaymentChange += SetExpenseInvoiceId;
            AmountChange += SetIncome;
            PaymentChange += SetIncome;
            _delitaTradeDayReport.CurentDayReportSelect += CurrendDayReportViewModelSelect;
            _delitaTradeDayReport.CurentDayReportSelect += OnCurentDayReportChanged;
            _delitaTradeDayReport.CurentDayReportSelect += OnSelectDayReportColorCanged;
            _delitaTradeDayReport.CurrentDayReportUnselected += CurrentDayReportViewModelUnselect;
            _delitaTradeDayReport.CurrentDayReportUnselected += OnCurentDayReportChanged;
            _delitaTradeDayReport.CurrentDayReportUnselected += UpdateDayReportsId;
            _delitaTradeDayReport.CurrentDayReportUnselected += ClearDayReportData;
            _delitaTradeDayReport.CurrentDayReportUnselected += OnUnselectDayReportColorCanged;
            _delitaTradeDayReport.DayReportsIdChanged += UpdateDayReportsId;
            UpdateDayReportsId();
            DayReportIdViewModel.DayReportIdExists += LoadDayReport;
            PropertyChanged += OnCurentViewModelPropertyChanged;
            LoadInvoice += LoadInvoiceFromList;
            _dayReporsId.CollectionChanged += OnDayReportIdColectionChange;
        }

        private void OnLoadInvoiceFromList()
        {
            LoadInvoice?.Invoke();
        }

        private void OnAmountChange()
        {
            AmountChange?.Invoke();
        }

        private void SetAmountToZero()
        {
            _amount = 0;
            OnPropertyChange(nameof(Amount));
        }

        private void OnCurentViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedInvoiceViewModel))
            {
                OnLoadInvoiceFromList();
            }
        }

        private void LoadInvoiceFromList()
        {
            if (SelectedInvoiceViewModel != null)
            {                
                int index = SelectedInvoiceViewModel.CompanyName.LastIndexOf(' ');
                
                SearchBox.InputText = SelectedInvoiceViewModel.CompanyName[..index];
                SearchBoxObject.InputTextObject = SelectedInvoiceViewModel.ObjectName;
                PayMethodBox.PayMethodText = SelectedInvoiceViewModel.PayMethod;
                InvoiceID = SelectedInvoiceViewModel.InvoiceID;
                Weight = SelectedInvoiceViewModel.Weight.ToString();
                Amount = SelectedInvoiceViewModel.Amount.ToString();
                Income = SelectedInvoiceViewModel.Income.ToString();
            }           
        }
              
        private void OnCurentDayReportChanged()
        {
            OnPropertyChange(nameof(DayReportId));            
        }

        private void UpdateDayReportsId()
        {
            _dayReporsId.Clear();
            foreach (var dayReportId in _delitaTradeDayReport.GetAllDayReportsID())
            {
                _dayReporsId.Add(dayReportId);
            }            
        }

        private void CurrendDayReportViewModelSelect()
        {
            _currentDayReportViewModel = new CurrentDayReportViewModel(_delitaTradeDayReport.DayReport);
        }

        private void CurrentDayReportViewModelUnselect()
        {
            _currentDayReportViewModel = new CurrentDayReportViewModelNull();
        }

        private void ClearDayReportData()
        {
            _invoices.Clear();
            OnPropertyChange(nameof(Invoices));
        }

        private void UpdateDayReportData()
        {            
            _invoices.Clear();
            IEnumerable<Invoice> invoices = _delitaTradeDayReport.GetAllInvoices()
                .OrderByDescending(i => i.PayMethod)
                .ThenBy(i => i.CompanyName)
                .ThenBy(i => i.InvoiceID);

            foreach (Invoice invoice in invoices)
            {
                InvoiceViewModel invoiceViewModel = new InvoiceViewModel(invoice);
                _invoices.Add(invoiceViewModel);
            }
            OnPropertyChange(nameof(Invoices));
        }

        private void LoadDayReport()
        {
            LoadDayReportCommand.Execute(null);
        }

        private void OnAddCompanyPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChange($"{e.PropertyName}");
        }

        private void PayMenthodBoxPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PayMethodBoxViewModel.PayMethodText))
            {
                PaymentStatusChange();
            }
        }

        private void PaymentStatusChange()
        {
            PaymentChange?.Invoke();
        }

        private void SetExpenseInvoiceId()
        {
            if (PayMethodBox.PayMethodText == "Разход" && !_isPayMethodLoad)
            {   
                for (int i = 0; i < 10; i++)
                {
                    string expandId = $"0{DateTime.Now.Date:ddMMyyyy}{i}";
                    if (Invoices.FirstOrDefault(i => i.InvoiceID == expandId) == null)
                    {
                        InvoiceID = expandId;
                        break;
                    }
                }
            }
            else 
            {
                _isPayMethodLoad = false;
            }
        }

        private bool CheckInvoceIdIsValid(string id)
        {
            return id.Length == 10 && id.All(char.IsDigit);
        }

        private void SetPayMethod()
        {
            if (_addNewCompanyViewModel.CurrentObject != null
                    && _addNewCompanyViewModel.CurrentObject.BankPay)
            {
                _payMethodBoxViewModel.PayMethodText = "Банка";
            }
            else
            {
                _payMethodBoxViewModel.PayMethodText = "В брой";
            }
                PaymentStatusChange();
        }

        private void SetIncome()
        {
            if (_payMethodBoxViewModel.PayMethodText == "В брой")
            {
                Income = $"{_amount}";
            }
            else if (_payMethodBoxViewModel.PayMethodText == "Разход")
            {
                SetAmountToZero();
            }
            else
            {
                Income = $"0";
            }
        }

        public string Date
        {
            get => _date.ToString("yyyy-MM-dd");
            set
            {
                try
                {
                    if (DateTime.TryParse(value, out _date) == false)
                    {
                        throw new ArgumentException("Day report date ID is incorrect"); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _date = DateTime.Now.Date;
                }                
                OnPropertyChange();
            }
        }

        public string DayReportColor => _dayReportColor;

        private string GetNextDayReportId()
        {
            return DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
        }

        private void OnDayReportIdColectionChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (_dayReporsId.Count > 0 && _dayReporsId[^1] == DateTime.Now.Date.ToString("yyyy-MM-dd"))
            {
                Date = GetNextDayReportId();
            }
        }
        private void OnSelectDayReportColorCanged()
        {
            _dayReportColor = "#FF3BEB23";
                
            OnPropertyChange(nameof(DayReportColor));
        }

        private void OnUnselectDayReportColorCanged()
        {
            _dayReportColor = "Red";

            OnPropertyChange(nameof(DayReportColor));
        }

        public string AddOrUpdateTextCommand
        {
            get => _addOrUpdateCommand;
            set 
            {
                _addOrUpdateCommand = value;
                OnPropertyChange();
            }
        }

        public string CompanyType
        {
            get => _addNewCompanyViewModel.CompanyType;
            set
            {
                _addNewCompanyViewModel.CompanyType = value;
                OnPropertyChange();
            }
        }

        public string Amount
        {
            get => $"{_amount:C2}";
            set
            {
                _amount = _currencyProvider.GetDecimalValue(value);
                OnAmountChange();
                OnPropertyChange();
            }
        }

        public decimal DecimalAmount => _amount;

        public string Income
        {
            get => $"{_income:C2}";
            set
            {
                _income = _currencyProvider.GetDecimalValue(value);
                SetIncomeViaPaymentStatus(PayMethodBox.PayMethodText);
                OnPropertyChange();
            }
        }

        public double DoubleWeight => _weight;

        public string Weight
        {
            get => $"{_weight:F2} kg."; 
            set 
            {
                double.TryParse(value, out _weight);
                OnPropertyChange();
            }
        }

        public decimal DecimalIncome => _income;

        private void IncomeIsZero()
        {
            _income = 0;
        }

        private void IncomIsPositive()
        {
            if (_income < 0)
            {
                _income = 0;
            }
        }

        private void IncomeIsNegative()
        {
            if (_income > 0) 
            {
                _income *= -1;
            }
        }
        
        private void SetIncomeViaPaymentStatus(string paymentStatus)
        {
            switch (paymentStatus) 
            {
                case "Банка":
                    IncomeIsZero();
                    break;
                case "В брой":
                case "Стара сметка":
                    IncomIsPositive();
                    break;
                case "С карта":
                    IncomIsPositive();
                    break;
                case "Кредитно":
                    IncomeIsNegative();
                    break;
                case "Разход":
                    IncomeIsNegative();
                    break;
                default:
                    break;
            }
        }
        
        public string InvoiceID
        {
            get => _previousInvoiceID;
            set
            {
                if (CheckInvoceIdIsValid(value))
                {
                    _invoiceID = value;
                    _previousInvoiceID = _invoiceID;
                    OnPropertyChange();
                }
            }
        }
       
        public ICommand AddOrUpdateInvoiceCommand { get; }

        public ICommand RemoveInvoiceCommand { get; }

        public ICommand CreateNewDayReportCommand { get;}

        public ICommand LoadDayReportCommand { get; }

        public ICommand DeleteDayReportCommand { get;}
    }
}
