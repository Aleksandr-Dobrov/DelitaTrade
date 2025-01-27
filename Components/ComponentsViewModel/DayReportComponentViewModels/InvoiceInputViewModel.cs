using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class InvoiceInputViewModel : IDisposable
    {
        private readonly CompaniesSearchViewModel _companiesViewModel;
        private readonly CompanyObjectsSearchViewModel _companyObjectsViewModel;
        private readonly LabeledStringTextBoxViewModel _companyTypeViewModel;
        private readonly LabeledInvoiceNumberViewModel _invoiceNumberViewModel;
        private readonly LabeledCurrencyViewModel _amountViewModel;
        private readonly LabeledPayMethodSelectableBoxViewModel _payMethodViewModel;
        private readonly LabeledCurrencyViewModel _incomeViewModel;

        public InvoiceInputViewModel(CompaniesSearchViewModel companiesViewModel, CompanyObjectsSearchViewModel companyObjectsViewModel, LabeledStringTextBoxViewModel companyTypeViewModel, LabeledInvoiceNumberViewModel invoiceNumberViewModel, LabeledCurrencyViewModel amountViewModel, LabeledPayMethodSelectableBoxViewModel payMethodViewModel, LabeledCurrencyViewModel incomeViewModel)
        {
            _companiesViewModel = companiesViewModel;
            _companyObjectsViewModel = companyObjectsViewModel;
            _companyTypeViewModel = companyTypeViewModel;
            _invoiceNumberViewModel = invoiceNumberViewModel;
            _amountViewModel = amountViewModel;
            _payMethodViewModel = payMethodViewModel;
            _incomeViewModel = incomeViewModel;
            SetViewModels();
            OnEnable();
        }

        public CompaniesSearchViewModel CompaniesViewModel => _companiesViewModel;

        public CompanyObjectsSearchViewModel CompanyObjectsViewModel => _companyObjectsViewModel;

        public LabeledStringTextBoxViewModel CompanyTypeViewModel => _companyTypeViewModel;

        public LabeledInvoiceNumberViewModel InvoiceNumberViewModel => _invoiceNumberViewModel;

        public LabeledCurrencyViewModel AmountViewModel => _amountViewModel;

        public LabeledPayMethodSelectableBoxViewModel PayMethodViewModel => _payMethodViewModel;

        public LabeledCurrencyViewModel IncomeViewModel => _incomeViewModel;

        public void Dispose()
        {
            OnDisable();
        }

        private void SetViewModels()
        {
            _companyTypeViewModel.Label = "Type";
            _incomeViewModel.Label = "Number";
            _amountViewModel.Label = "Amount";
            _payMethodViewModel.Label = "PayMethod";
            _incomeViewModel.Label = "Income";
            _companyTypeViewModel.SetDefaultValue();
        }
        private void OnEnable()
        {
            CompaniesViewModel.ValueSelected += OnSelectedCompany;
            CompaniesViewModel.ValueUnselected += OnUnSelectedCompany;
            CompaniesViewModel.ValueUnselected += RestoreCompanyInputData;
            CompanyObjectsViewModel.ValueSelected += LoadObjectInputData;
            CompanyObjectsViewModel.ValueUnselected += OnUnselectedCompanyObject;
        }

        private void OnDisable()
        {
            CompaniesViewModel.ValueSelected -= OnSelectedCompany;
            CompaniesViewModel.ValueUnselected -= OnUnSelectedCompany;
            CompaniesViewModel.ValueUnselected -= RestoreCompanyInputData;
            CompanyObjectsViewModel.ValueSelected -= LoadObjectInputData;
            CompanyObjectsViewModel.ValueUnselected -= OnUnselectedCompanyObject;
        }
        private void OnSelectedCompany(CompanyViewModel company)
        {
            CompanyObjectsViewModel.SelectCompanyReference(company.Id);
            _companyTypeViewModel.TextBox = company.Type ?? string.Empty;
            CompanyObjectsViewModel.CheckCompanyReference(company.Id);
            //if (CompanyObjectsViewModel.CompanyObjectsSearchBox.Value.Value?.Company.Id != company.Id)
            //{
            //    CompanyObjectsViewModel.CompanyObjectsSearchBox.AddError("TextValue", "Object not found");
            //}
            //else 
            //{
            //    CompanyObjectsViewModel.CompanyObjectsSearchBox.ClearErrors("TextValue");
            //}
        }

        private void OnUnSelectedCompany()
        {
            CompanyObjectsViewModel.UnSelectCompanyReference();
        }

        private void OnUnselectedCompanyObject()
        {
            CompanyObjectsViewModel.UnSelectCompanyReference();
            CompaniesViewModel.UnSelectCompany();
        }
       
        private void RestoreCompanyInputData()
        {
            _companyTypeViewModel.SetDefaultValue();
        }
                
        private void LoadObjectInputData(CompanyObjectViewModel companyObject)
        {
            if (CompaniesViewModel.CompaniesSearchBox.Value.Value == null
                || companyObject.Company.Id != CompaniesViewModel.CompaniesSearchBox.Value.Value.Id)
            {
                CompaniesViewModel.CompaniesSearchBox.SetSelectedValue(companyObject.Company);
                _companyTypeViewModel.TextBox = companyObject.Company.Type ?? string.Empty;
            }
        }
    }
}
