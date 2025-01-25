﻿using DelitaTrade.Components.ComponentsView;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.WpfViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.ViewModels.Controllers
{
    public class CompaniesDataManager : IDisposable
    {
        private readonly CompaniesSearchViewModel _companies;
        private readonly CompanyObjectsSearchViewModel _companyObjects;
        private CompaniesDataViewModel _companyData;
        private CompanyCommandsViewModel _companyCommands;
        private CompanyObjectCommandsViewModel _companyObjectCommands;
        private TradersListViewModel _tradersListViewModel;
        private WpfCompanyViewModel _wpfCompanyViewModel;
        private WpfCompanyObjectViewModel _wpfCompanyObjectViewModel;

        public CompaniesDataManager(CompaniesSearchViewModel companies, CompanyObjectsSearchViewModel companyObjects, CompaniesDataViewModel companyDataViewModel, CompanyCommandsViewModel companyCommandsViewModel, CompanyObjectCommandsViewModel companyObjectCommands, TradersListViewModel tradersListViewModel, WpfCompanyViewModel wpfCompanyViewModel, WpfCompanyObjectViewModel wpfCompanyObjectViewModel)
        {
            _companies = companies;
            _companyObjects = companyObjects;
            _companyData = companyDataViewModel;
            _companyCommands = companyCommandsViewModel;
            _companyObjectCommands = companyObjectCommands;
            _tradersListViewModel = tradersListViewModel;
            _wpfCompanyViewModel = wpfCompanyViewModel;
            _wpfCompanyObjectViewModel = wpfCompanyObjectViewModel;
            OnEnable();
            _companyCommands.CreateCommands(Companies, CompanyObjects, WpfCompanyViewModel);
            _companyObjectCommands.CreateCommands(Companies, CompanyObjects, WpfCompanyObjectViewModel, TradersViewModel);
        }

        public CompaniesSearchViewModel Companies => _companies;
        public WpfCompanyViewModel WpfCompanyViewModel => _wpfCompanyViewModel;
        public CompanyCommandsViewModel CompanyCommands => _companyCommands;

        public CompanyObjectsSearchViewModel CompanyObjects => _companyObjects;
        public CompaniesDataViewModel CompanyData => _companyData;
        public WpfCompanyObjectViewModel WpfCompanyObjectViewModel => _wpfCompanyObjectViewModel;
        public CompanyObjectCommandsViewModel CompanyObjectCommands => _companyObjectCommands;


        public TradersListViewModel TradersViewModel => _tradersListViewModel;

        public void Dispose()
        {
            OnDisable();
        }

        private void OnSelectedCompany(Core.ViewModels.CompanyViewModel company)
        {
            CompanyObjects.SelectCompanyReference(company.Id);
        }

        private void OnUnSelectedCompany()
        {
            CompanyObjects.UnSelectCompanyReference();
        }
        private void RestoreObjectInputData()
        {
            WpfCompanyObjectViewModel.UnSelectViewModel();
        }

        private void RestoreCompanyInputData()
        {
            WpfCompanyViewModel.UnSelectViewModel();
            WpfCompanyObjectViewModel.UnSelectCompany();
        }

        private void LoadCompanyInputData(Core.ViewModels.CompanyViewModel company)
        {
            WpfCompanyViewModel.SelectViewModel(company);
            WpfCompanyObjectViewModel.SelectCompany(company);
        }

        private void LoadObjectInputData(Core.ViewModels.CompanyObjectViewModel companyObject)
        {
            if (Companies.CompaniesSearchBox.Value.Value == null
                || companyObject.Company.Id != Companies.CompaniesSearchBox.Value.Value.Id)
            {
                Companies.CompaniesSearchBox.SetSelectedValue(companyObject.Company);
                WpfCompanyViewModel.SelectViewModel(companyObject.Company);
            }
            WpfCompanyObjectViewModel.SelectViewModel(companyObject);
            TradersViewModel.TraderViewModel.TextValue = ((CompanyObjectDeepViewModel)companyObject).Trader.Name;
        }

        private void OnEnable()
        {
            Companies.ValueSelected += OnSelectedCompany;
            Companies.ValueUnselected += OnUnSelectedCompany;
            CompanyObjects.ValueUnselected += RestoreObjectInputData;
            Companies.ValueUnselected += RestoreCompanyInputData;
            CompanyObjects.ValueSelected += LoadObjectInputData;
            Companies.ValueSelected += LoadCompanyInputData;
        }

        private void OnDisable()
        {
            Companies.ValueSelected -= OnSelectedCompany;
            Companies.ValueUnselected -= OnUnSelectedCompany;
            CompanyObjects.ValueUnselected -= RestoreObjectInputData;
            Companies.ValueUnselected -= RestoreCompanyInputData;
            CompanyObjects.ValueSelected -= LoadObjectInputData;
            Companies.ValueSelected -= LoadCompanyInputData;
        }
    }
}
