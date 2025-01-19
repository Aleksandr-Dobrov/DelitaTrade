using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using DelitaTrade.Core.Contracts;
using System.Windows.Input;
using Microsoft.IdentityModel.Tokens;
using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.ViewModels.Interfaces;

namespace DelitaTrade.ViewModels.Controllers
{
    public class CompaniesSearchViewModel
    {
        private SearchComboBoxViewModel<Core.ViewModels.CompanyViewModel> _companiesSearchBox;
        private IServiceProvider _serviceProvider;

        public CompaniesSearchViewModel(IServiceProvider service)
        {
            _serviceProvider = service;
            _companiesSearchBox = new SearchComboBoxViewModel<DelitaTrade.Core.ViewModels.CompanyViewModel>();
            _companiesSearchBox.PropertyChanged += OnViewModelPropertyChange;
            _companiesSearchBox.Name = "Companies";
            ValueSelected += (Core.ViewModels.CompanyViewModel obj) => { CompaniesSearchBox.Value.Value.CompanyObjects = obj.CompanyObjects; };
            ValueUnselected += () => { };
        }

        public SearchComboBoxViewModel<Core.ViewModels.CompanyViewModel> CompaniesSearchBox => _companiesSearchBox;

        public event Action<Core.ViewModels.CompanyViewModel> ValueSelected;
        public event Action ValueUnselected;

        public void UpdateSelectedCompany(ICompanyData companyData)
        {
            CompaniesSearchBox.Value.Value.Type = companyData.CompanyType;
            CompaniesSearchBox.Value.Value.Bulstad = companyData.Bulstad;
        }

        private async void ReloadAllCompanies()
        {
            using var scope = _serviceProvider.CreateScope();
            var companyService = scope.GetService<ICompanyService>();
            var companies = await companyService.GetAllAsync();
            CompaniesSearchBox.UpdateItems(companies.OrderBy(c => c.Name));
        }

        private async Task LoadFilteredCompanies(string arg)
        {
            if(CompaniesSearchBox.Items.FirstOrDefault(c => c.Name == arg) != null) return;
            using var scope = _serviceProvider.CreateScope();
            var companyService = scope.GetService<ICompanyService>();
            var companies = (await companyService
                .GetFilteredAsync(arg, limit: 20));
            CompaniesSearchBox.UpdateItems(companies);
        }

        private async Task<Core.ViewModels.CompanyViewModel> LoadCompanyById(int CompanyId)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<ICompanyService>();
            var company = await service.GetDetailedCompanyByIdAsync(CompanyId);
            return company;
        }

        private async void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CompaniesSearchBox.TextValue))
            {
                 await LoadFilteredCompanies(CompaniesSearchBox.TextValue);
            }
            if (e.PropertyName == nameof(CompaniesSearchBox.Value.Value))
            {
                if (CompaniesSearchBox.Value.Value != null)
                {
                    ValueSelected(await LoadCompanyById(CompaniesSearchBox.Value.Value.Id));
                }
                else if (CompaniesSearchBox.TextValue == string.Empty)
                {
                    ValueUnselected();
                }
            }
        }
    }
}
