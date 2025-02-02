using DelitaTrade.Commands;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels;
using DelitaTrade.Components.ComponentsViewModel.OptionsComponentViewModels;
using DelitaTrade.Models;
using DelitaTrade.Models.Configurations;
using DelitaTrade.Models.DataProviders;
using DelitaTrade.Models.DataProviders.FileDirectoryProvider;
using DelitaTrade.ViewModels.Controllers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;

namespace DelitaTrade.ViewModels
{
    public class DayReportsViewModel : ViewModelBase
    {
        private const string _deleteDayReportButtonImage = "Components\\ComponentAssets\\DayReport\\delete-file_40456.png";
        private const string _createDayReportButtonImage = "Components\\ComponentAssets\\DayReport\\add-document.png";
        private const string _addInvoiceButtonImage = "Components\\ComponentAssets\\DayReport\\add.png";
        private const string _updateInvoiceButtonImage = "Components\\ComponentAssets\\DayReport\\update.png";
        private const string _deleteInvoiceButtonImage = "Components\\ComponentAssets\\DayReport\\remove.png";

        private const string _defaultInvoiceID = "1000000000";
        private const string _lightGreenColor = "#FF90EE90";
        private const string _orangeYellowColor = "#FFFFBD00";
        private const string _unselectDayReportColor = "Red";
        private const string _selectDayReportColor = "#FF3BEB23";

        private CurrencyProvider _currencyProvider;
        private AddNewCompanyViewModel _addNewCompanyViewModel;
        private PayMethodBoxViewModel _payMethodBoxViewModel;
        private Components.ComponentsViewModel.DayReportTotalsViewModel _dayReportTotalsViewModel;
        private ObservableCollection<InvoiceViewModel> _invoices;
        private InvoiceViewModel _selectedInvoiceViewModel;
        private DelitaTradeDayReport _delitaTradeDayReport;
        private CurrentDayReportViewModel _currentDayReportViewModel;
        private DayReportIdViewModel _dayReportIdViewModel;
        private DateTime _date = DateTime.Now.Date;
        private ObservableCollection<string> _dayReporsId;
        private DayReportInputOptionsViewModelComponent _dayReportinputOptions;
        private LabeledInvoiceNumberViewModel _labeledInvoiceNumberViewModel;
        private LabeledCurrencyViewModel _labeledCurrencyViewModel = new();
        private LabeledPayMethodSelectableBoxViewModel _labeledPayMethodSelectableBoxViewModel = new();
        private InvoiceInputViewModel _invoiceInputViewModel;

        private string _addOrUpdateCommand = "Add";
        private double _weight;
        private decimal _income;
        private decimal _amount;
        private string _invoiceID = _defaultInvoiceID;
        private string _dayReportColor = _unselectDayReportColor;
        private string _addButtonColor = _lightGreenColor;
        private bool _isPayMethodLoad = false;
        private bool _incomeEnable = false;

        public DayReportsViewModel(DelitaTradeDayReport delitaTradeDayReport, ViewModelBase addNewCompanyViewModel, DayReportInputOptionsViewModelComponent options, IServiceProvider serviceProvider)
        {
            _currencyProvider = new CurrencyProvider();
            _delitaTradeDayReport = delitaTradeDayReport;
            _addNewCompanyViewModel = (AddNewCompanyViewModel?)addNewCompanyViewModel;
            _payMethodBoxViewModel = new PayMethodBoxViewModel();
            _dayReporsId = new ObservableCollection<string>();
            _currentDayReportViewModel = new CurrentDayReportViewModelNull();
            _dayReportTotalsViewModel = new Components.ComponentsViewModel.DayReportTotalsViewModel(delitaTradeDayReport);
            _invoices = new ObservableCollection<InvoiceViewModel>();
            AddInvoiceCommand = new AddInvoiceCommand(_delitaTradeDayReport, _addNewCompanyViewModel, this);
            UpdateInvoiceCommand = new UpdateInvoiceCommand(_delitaTradeDayReport, _addNewCompanyViewModel, this);
            _dayReportIdViewModel = new DayReportIdViewModel(delitaTradeDayReport.DayReportIdDataBese);
            CreateNewDayReportCommand = new CreateNewDayReportCommand(_delitaTradeDayReport, this);
            LoadDayReportCommand = new LoadDayReportCommand(_delitaTradeDayReport, this);
            DeleteDayReportCommand = new DeleteDayReportCommand(_delitaTradeDayReport, this);
            RemoveInvoiceCommand = new RemoveInvoiceCommand(_delitaTradeDayReport, this);
            _labeledInvoiceNumberViewModel = new LabeledInvoiceNumberViewModel();
            _labeledInvoiceNumberViewModel.Label = "Number";
            _dayReportinputOptions = options;
            _invoiceInputViewModel = serviceProvider.GetService<InvoiceInputViewModel>();
            //_dayReportinputOptions.SetWeightConfigurator(delitaTradeDayReport.AppConfig);
            OnEnable();
        }

        public event Action PaymentChange;
        public event Action LoadInvoice;
        public event Action AmountChange;
        public event Action InvoiceColectionChange;        
        
        public IEnumerable<string> DayReportsId => _dayReporsId;
        public IEnumerable<InvoiceViewModel> Invoices => _invoices;
        public CompaniesSearchViewModel SearchBox => _addNewCompanyViewModel.CompaniesDataManager.Companies;
        public CompanyObjectsSearchViewModel SearchBoxObject => _addNewCompanyViewModel.CompaniesDataManager.CompanyObjects;
        public PayMethodBoxViewModel PayMethodBox => _payMethodBoxViewModel;
        public Components.ComponentsViewModel.DayReportTotalsViewModel DayReportTotalsViewModel => _dayReportTotalsViewModel;
        public DayReportIdViewModel DayReportIdViewModel => _dayReportIdViewModel;
        public CurrentDayReportViewModel CurrentDayReportViewModel => _currentDayReportViewModel;
        public DayReportInputOptionsViewModelComponent DayReportInputOptions => _dayReportinputOptions;
        public LabeledInvoiceNumberViewModel LabeledInvoiceNumberViewModel => _labeledInvoiceNumberViewModel;
        public LabeledCurrencyViewModel LabeledCurrencyViewModel => _labeledCurrencyViewModel;
        public LabeledPayMethodSelectableBoxViewModel LabeledPayMethodSelectableBoxViewModel => _labeledPayMethodSelectableBoxViewModel;
        public InvoiceInputViewModel InputViewModel => _invoiceInputViewModel;
        public string LoadDayReportId => _dayReportIdViewModel.DayReportId;
        public string DayReportId => _currentDayReportViewModel.DayReportId;
        public string DayReportColor => _dayReportColor;
        public decimal DecimalAmount => _amount;
        public decimal DecimalIncome => _income;
        public double DoubleWeight => _weight;

        public string DeleteDayReportButtonImage => _deleteDayReportButtonImage.GetFullFilePath();
        public string CreateDayReportButtonImage => _createDayReportButtonImage.GetFullFilePath();
        public string AddInvoiceButtonImage => _addInvoiceButtonImage.GetFullFilePath();
        public string UpdateInvoiceButtonImage => _updateInvoiceButtonImage.GetFullFilePath();
        public string DeleteInvoiceButtonImage => _deleteInvoiceButtonImage.GetFullFilePath();

        public InvoiceViewModel SelectedInvoiceViewModel
        {
            get { return _selectedInvoiceViewModel; }
            set
            {
                _selectedInvoiceViewModel = value;
                OnPropertyChange();
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

        public string InvoiceID
        {
            get => _invoiceID;
            set
            {
                if (CheckInvoceIdIsValid(value))
                {
                    _invoiceID = value;
                    OnPropertyChange();
                }
            }
        }

        public string AddButtonColor
        {
            get => _addButtonColor;
            set
            {
                _addButtonColor = value;
                OnPropertyChange();
            }
        }

        public string CompanyType
        {
            get => _addNewCompanyViewModel.CompaniesDataManager.CompanyData.CompanyType;
            set
            {
                _addNewCompanyViewModel.CompaniesDataManager.CompanyData.CompanyType = value;
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

        public string Weight
        {
            get => $"{_weight:F2} kg.";
            set
            {
                double.TryParse(value, out _weight);
                OnPropertyChange();
            }
        }

        public bool IncomeEnable
        {
            get => _incomeEnable;
            private set 
            {
                _incomeEnable = value;
                OnPropertyChange();
            }
        }

        public bool IsUnpaidInvoice(string invoiceId)
        {
            if (invoiceId != null && IsNewInvoice(invoiceId) == false)
            {
                return _delitaTradeDayReport.CheckIsUnpaidInvoice(invoiceId);
            }
            return false;
        }

        public bool IsNewInvoice(string invoiceId)
        {
            if (invoiceId == null) return false;
            return _delitaTradeDayReport.CheckIsNewInvoice(invoiceId);
        }

        public ICommand AddInvoiceCommand { get; }
        public ICommand RemoveInvoiceCommand { get; }
        public ICommand CreateNewDayReportCommand { get; }
        public ICommand LoadDayReportCommand { get; }
        public ICommand DeleteDayReportCommand { get; }
        public ICommand UpdateInvoiceCommand { get; }

        private void OnEnable()
        {
            _addNewCompanyViewModel.PropertyChanged += OnAddCompanyPropertyChanged;
            _delitaTradeDayReport.DayReportDataChanged += UpdateDayReportData;
            _delitaTradeDayReport.DayReportDataChanged += SetAddButonColor;
            //_addNewCompanyViewModel.ObjectSelected += SetPayMethod;
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
            _invoices.CollectionChanged += OnInvoiceColectionChange;
            InvoiceColectionChange += () => { };
            PaymentChange += SetIncomeTextBoxEnable;
            _dayReportinputOptions.PropertyChanged += OnComponentPropertyChange;
            _delitaTradeDayReport.ExportFileCreate += OpenExportFile;
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

        private void OnComponentPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            OnPropertyChange(nameof(e.PropertyName));
        }

        private void OnCurentViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedInvoiceViewModel))
            {
                OnLoadInvoiceFromList();
            }
            else if (e.PropertyName == nameof(InvoiceID))
            {
                SetAddButonColor();
            }
        }

        private void SetAddButonColor()
        {
            if (IsUnpaidInvoice(InvoiceID))
            {
                AddButtonColor = _orangeYellowColor;
            }
            else
            {
                AddButtonColor = _lightGreenColor;
            }
        }

        private void LoadInvoiceFromList()
        {
            if (SelectedInvoiceViewModel != null)
            {
                int index = SelectedInvoiceViewModel.CompanyName.LastIndexOf(' ');

                //SearchBox.InputText = SelectedInvoiceViewModel.CompanyName[..index];
                //SearchBoxObject.InputTextObject = SelectedInvoiceViewModel.ObjectName;
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
            SetDayReportIdNextDate();
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
                .ThenBy(i => i.CompanyFullName)
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

        //private void SetPayMethod()
        //{
        //    if (_addNewCompanyViewModel.CurrentObject != null
        //            && _addNewCompanyViewModel.CurrentObject.BankPay)
        //    {
        //        _payMethodBoxViewModel.PayMethodText = "Банка";
        //    }
        //    else
        //    {
        //        _payMethodBoxViewModel.PayMethodText = "В брой";
        //    }
        //    PaymentStatusChange();
        //}

        private void SetIncome()
        {
            if (_payMethodBoxViewModel.PayMethodText == "В брой" || 
                _payMethodBoxViewModel.PayMethodText == "С карта" ||
                _payMethodBoxViewModel.PayMethodText == "Стара сметка" ||
                _payMethodBoxViewModel.PayMethodText == "Кредитно")
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

        private string GetNextDayReportId()
        {
            return DateTime.Today.AddDays(1).ToString("yyyy-MM-dd");
        }

        private void OnInvoiceColectionChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            InvoiceColectionChange();
        }

        private void OnDayReportIdColectionChange(object? sender, NotifyCollectionChangedEventArgs e)
        {
            SetDayReportIdNextDate();
        }

        private void SetDayReportIdNextDate()
        {
            if (_dayReporsId.Count > 0 && _dayReporsId[^1] == DateTime.Now.Date.ToString("yyyy-MM-dd"))
            {
                Date = GetNextDayReportId();
            }
        }

        private void OnSelectDayReportColorCanged()
        {
            _dayReportColor = _selectDayReportColor;

            OnPropertyChange(nameof(DayReportColor));
        }

        private void OnUnselectDayReportColorCanged()
        {
            _dayReportColor = _unselectDayReportColor;

            OnPropertyChange(nameof(DayReportColor));
        }

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

        private void SetIncomeTextBoxEnable()
        {
            if (PayMethodBox.PayMethodText == "Банка")
            {
                IncomeEnable = false;
            }
            else 
            {
                IncomeEnable = true;
            }
        }

        private void OpenExportFile(string filePath) 
        {
            MessageBoxResult boxResult = MessageBox.Show($"Day report exported successful.{Environment.NewLine}Open file?", "Exporter"
                                                             , MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (boxResult == MessageBoxResult.Yes)
            {
                Process.Start("explorer.exe", filePath);
            }
        }
    }
}
