﻿using DelitaTrade.Common.Enums;
using DelitaTrade.Core.Interfaces;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.ViewModels.Controllers;

namespace DelitaTrade.Components.ComponentsViewModel.DayReportComponentViewModels
{
    public class InvoiceCompaniesInputViewModel : IDisposable
    {
        private readonly CompaniesSearchViewModel _companiesViewModel;
        private readonly CompanyObjectsSearchViewModel _companyObjectsViewModel;
        private readonly LabeledStringTextBoxViewModel _companyTypeViewModel;

        public InvoiceCompaniesInputViewModel(CompaniesSearchViewModel companiesViewModel, CompanyObjectsSearchViewModel companyObjectsViewModel, LabeledStringTextBoxViewModel companyTypeViewModel)
        {
            _companiesViewModel = companiesViewModel;
            _companyObjectsViewModel = companyObjectsViewModel;
            _companyTypeViewModel = companyTypeViewModel;
            OnCompanyObjectIsBankChange += (o) => { };
            SetViewModels();
            OnEnable();
        }

        public event Action<ICompanyObjectIsBankPay> OnCompanyObjectIsBankChange;

        public CompaniesSearchViewModel CompaniesViewModel => _companiesViewModel;

        public CompanyObjectsSearchViewModel CompanyObjectsViewModel => _companyObjectsViewModel;

        public LabeledStringTextBoxViewModel CompanyTypeViewModel => _companyTypeViewModel;

        public bool HasError => _companiesViewModel.CompaniesSearchBox.HasErrors || _companyObjectsViewModel.CompanyObjectsSearchBox.HasErrors || _companyTypeViewModel.HasErrors;

        public void Dispose()
        {
            OnDisable();
        }

        private void SetViewModels()
        {
            _companyTypeViewModel.Label = "Type";
            _companyTypeViewModel.SetDefaultValue();
        }
        
        private void OnSelectedCompany(CompanyViewModel company)
        {
            CompanyObjectsViewModel.SelectCompanyReference(company.Id);
            _companyTypeViewModel.TextBox = company.Type ?? string.Empty;
            CompanyObjectsViewModel.CheckCompanyReference(company.Id);
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
            OnCompanyObjectIsBankChange(companyObject);
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
    }
}
