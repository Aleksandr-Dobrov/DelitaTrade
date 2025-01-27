using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using System.Windows.Threading;
using Org.BouncyCastle.Utilities;
using DelitaTrade.ViewModels.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace DelitaTrade.ViewModels.Controllers
{
    public class CompanyObjectsSearchViewModel
    {
        private bool _isCompanyReference;
        private int _companyId = -1;
        private SearchComboBoxViewModel<Core.ViewModels.CompanyObjectViewModel> _companyObjectsSearchBox;
        private IServiceProvider _serviceProvider;

        public CompanyObjectsSearchViewModel(IServiceProvider service)
        {
            _serviceProvider = service;
            _companyObjectsSearchBox = new SearchComboBoxViewModel<Core.ViewModels.CompanyObjectViewModel>();
            _companyObjectsSearchBox.PropertyChanged += OnViewModelPropertyChange;
            //ReloadAllCompanies();
            _companyObjectsSearchBox.Name = "Company Objects";
            ValueUnselected += () => { };
            ValueSelected += (Core.ViewModels.CompanyObjectViewModel obj) =>{ CompanyObjectsSearchBox.Value.Value.Trader = obj.Trader; };
        }

        public SearchComboBoxViewModel<Core.ViewModels.CompanyObjectViewModel> CompanyObjectsSearchBox => _companyObjectsSearchBox;

        public event Action<Core.ViewModels.CompanyObjectViewModel> ValueSelected;
        public event Action ValueUnselected;

        public void SelectCompanyReference(int companyId)
        {
            _companyId = companyId;
            _isCompanyReference = true;
        }

        public void UnSelectCompanyReference()
        {
            _isCompanyReference = false;
            _companyId = -1;
        }

        public void CheckCompanyReference(int companyId)
        {
            if(CompanyObjectsSearchBox.Value.Value?.Company.Id != companyId)
            {
                CompanyObjectsSearchBox.AddError(nameof(CompanyObjectsSearchBox.TextValue), "The object does not belong to this company.");
            }
            else
            {
                CompanyObjectsSearchBox.ClearErrors(nameof(CompanyObjectsSearchBox.TextValue));
            }
        }
        public void UpdateSelectedCompany(ICompanyObjectData companyObjectData, ITraderData traderData)
        {
            var companyObject = _companyObjectsSearchBox.Value.Value;
            companyObject.IsBankPay = companyObjectData.BankPay;
            companyObject.Trader = traderData.Trader;
            if (companyObject.Address == null)
            {
                companyObject.Address = new() { Town = companyObjectData.Town };
            }
            else
            {
                companyObject.Address.Town = companyObjectData.Town;
            }
            companyObject.Address.StreetName = companyObjectData.Street;
            companyObject.Address.Number = companyObjectData.Number;
            companyObject.Address.GpsCoordinates = companyObjectData.GpsCoordinates;
            companyObject.Address.Description = companyObjectData.Description;
        }

        private async void ReloadAllObjects()
        {
            using var scope = _serviceProvider.CreateScope();
            var companyObjectService = scope.GetService<ICompanyObjectService>();
            var companies = await companyObjectService.GetAllAsync();
            CompanyObjectsSearchBox.UpdateItems(companies.OrderBy(c => c.Name));
        }

        private async Task LoadFilteredObjects(string arg)
        {            
            var splitArg = arg.Split(Core.ViewModels.CompanyObjectViewModel.DataSeparator)[0];
            if (CompanyObjectsSearchBox.Items.FirstOrDefault(c => c.Name == splitArg) != null) return;
            using var scope = _serviceProvider.CreateScope();
            var companyObjectService = scope.GetService<ICompanyObjectService>();
            var objects = Task.Run(() => companyObjectService
                .GetFilteredAsync(splitArg, limit: 40));
            await objects;
            CompanyObjectsSearchBox.UpdateItems(objects.Result);
        }

        private async Task LoadFilteredObjects(string arg, int companyId)
        {            
            var splitArg = arg.Split(Core.ViewModels.CompanyObjectViewModel.DataSeparator)[0];
            if (CompanyObjectsSearchBox.Items.FirstOrDefault(c => c.Name == splitArg) != null) return;
            using var scope = _serviceProvider.CreateScope();
            var companyObjectService = scope.GetService<ICompanyObjectService>();
            var objects = await companyObjectService
                .GetFilteredAsync(splitArg, companyId, limit: 40);
            CompanyObjectsSearchBox.UpdateItems(objects);
        }

        private async Task<CompanyObjectDeepViewModel> LoadObjectById(int ObjectId)
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.GetService<ICompanyObjectService>();
            var obj = await service.GetDetailedByIdAsync(ObjectId);
            return new CompanyObjectDeepViewModel
            {
                Id = ObjectId,
                Name = obj.Name,
                Address = obj.Address,
                IsBankPay = obj.IsBankPay,
                Company = obj.Company,
                Trader = obj.Trader,
            };
        }

        private async void OnViewModelPropertyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CompanyObjectsSearchBox.TextValue))
            {
                if (_isCompanyReference)
                {
                    await LoadFilteredObjects(CompanyObjectsSearchBox.TextValue, _companyId);
                }
                else
                {    
                     await LoadFilteredObjects(CompanyObjectsSearchBox.TextValue);
                }
            }
            if (e.PropertyName == nameof(CompanyObjectsSearchBox.Value.Value))
            {
                if (CompanyObjectsSearchBox.Value.Value != null)
                {
                    ValueSelected(await LoadObjectById(CompanyObjectsSearchBox.Value.Value.Id));
                }
                else if (CompanyObjectsSearchBox.TextValue.IsNullOrEmpty())
                {
                    ValueUnselected();
                }
            }
        }
    }
}
