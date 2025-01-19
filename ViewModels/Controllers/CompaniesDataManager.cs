using DelitaTrade.Components.ComponentsView;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Infrastructure.Data.Models;
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

        public CompaniesDataManager(CompaniesSearchViewModel companies, CompanyObjectsSearchViewModel companyObjects, CompaniesDataViewModel companyDataViewModel, CompanyCommandsViewModel companyCommandsViewModel, CompanyObjectCommandsViewModel companyObjectCommands, TradersListViewModel tradersListViewModel)
        {
            _companies = companies;
            _companyObjects = companyObjects;
            _companyData = companyDataViewModel;
            _companyCommands = companyCommandsViewModel;
            _companyObjectCommands = companyObjectCommands;
            _tradersListViewModel = tradersListViewModel;
            OnEnable();
            _companyCommands.CreateCommands(Companies, CompanyObjects, CompanyData);
            _companyObjectCommands.CreateCommands(Companies, CompanyObjects, CompanyData, TradersViewModel);
        }

        public CompaniesSearchViewModel Companies => _companies;
        public CompanyObjectsSearchViewModel CompanyObjects => _companyObjects;
        public CompaniesDataViewModel CompanyData => _companyData;

        public CompanyCommandsViewModel CompanyCommands => _companyCommands;
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
            CompanyData.RestoreObjectInputData();
        }

        private void RestoreCompanyInputData()
        {
            CompanyData.RestoreCompanyInputData();
        }

        private void LoadCompanyInputData(Core.ViewModels.CompanyViewModel company)
        {
            CompanyData.CompanyType = company.Type ?? string.Empty;
            CompanyData.Bulstad = company.Bulstad ?? string.Empty;
            //if (company.CompanyObjects.Count > 0)
            //{
            //    CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.UpdateItems(company.CompanyObjects);
            //    CompaniesDataManager.CompanyObjects.CompanyObjectsSearchBox.Value.Value = company.CompanyObjects[0];
            //}

        }

        private void LoadObjectInputData(Core.ViewModels.CompanyObjectViewModel companyObject)
        {
            CompanyData.CompanyType = companyObject.Company.Type ?? string.Empty;
            CompanyData.Bulstad = companyObject.Company.Bulstad ?? string.Empty;
            if (companyObject.Address != null)
            {
                CompanyData.Town = companyObject.Address.Town;
                CompanyData.Street = companyObject.Address.StreetName; //?? string.Empty;
                CompanyData.Number = companyObject.Address.Number; //?? string.Empty;
                CompanyData.Description = companyObject.Address.Description; //?? string.Empty;
                CompanyData.AddressViewModel = companyObject.Address;
                CompanyData.GpsCoordinates = companyObject.Address.GpsCoordinates; //?? string.Empty;
            }
            CompanyData.BankPay = companyObject.IsBankPay;
            if (Companies.CompaniesSearchBox.Value.Value == null
                || companyObject.Company.Id != Companies.CompaniesSearchBox.Value.Value.Id)
            {
                Companies.CompaniesSearchBox.SetSelectedValue(companyObject.Company);
            }
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
