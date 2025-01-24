using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Extensions;
using DelitaTrade.Models.Loggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DelitaTrade.ViewModels.Controllers
{
    public class CompanyObjectCommandsViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private CompanyObjectsSearchViewModel _searchViewModel;
        private CompaniesDataViewModel _dataViewModel;
        private CompaniesSearchViewModel _companiesSearchView;
        private TradersListViewModel _tradersViewModel;
        public CompanyObjectCommandsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public void CreateCommands(CompaniesSearchViewModel companiesSearch, CompanyObjectsSearchViewModel searchViewModel, CompaniesDataViewModel dataViewModel, TradersListViewModel tradersViewModel)
        {
            _searchViewModel = searchViewModel;
            _dataViewModel = dataViewModel;
            _companiesSearchView = companiesSearch;
            _tradersViewModel = tradersViewModel;
            CreateCommand = new DefaultCommand(CreateObject, CanCreateObject, _searchViewModel.CompanyObjectsSearchBox, _companiesSearchView.CompaniesSearchBox, _tradersViewModel.TraderViewModel, nameof(_searchViewModel.CompanyObjectsSearchBox.TextValue), nameof(_searchViewModel.CompanyObjectsSearchBox.Value.Value), nameof(_companiesSearchView.CompaniesSearchBox.Value.Value), nameof(_tradersViewModel.TraderViewModel.Value));
            UpdateCommand = new DefaultCommand(UpdateObject, CanUpdateObject, _dataViewModel, _tradersViewModel.TraderViewModel, nameof(_dataViewModel.Town), nameof(_dataViewModel.Street), nameof(_dataViewModel.Number), nameof(_dataViewModel.GpsCoordinates), nameof(_dataViewModel.Description), nameof(_dataViewModel.BankPay), nameof(_tradersViewModel.TraderViewModel.Value));
            DeleteCommand = new DefaultCommand(DeleteObject, CanDeleteObject, _searchViewModel.CompanyObjectsSearchBox, nameof(_searchViewModel.CompanyObjectsSearchBox.Value.Value));
        }
        private async Task CreateObject()
        {
            try
            {
                if (CanCreateObject())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ICompanyObjectService>();
                    Core.ViewModels.CompanyObjectDeepViewModel newCompanyObject = new()
                    {
                        Name = _searchViewModel.CompanyObjectsSearchBox.TextValue,
                        Company = _companiesSearchView.CompaniesSearchBox.Value.Value,
                        Trader = _tradersViewModel.TraderViewModel.Value.Value,
                        IsBankPay = _dataViewModel.BankPay,
                        Address = _dataViewModel.GetAddress()
                    };
                    newCompanyObject.Id = await service.CreateAsync(newCompanyObject);
                    _searchViewModel.CompanyObjectsSearchBox.Add(newCompanyObject);
                    _searchViewModel.CompanyObjectsSearchBox.SelectItem(newCompanyObject);
                }
                else throw new ArgumentException("Not Created");
            }
            catch (ArgumentException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning).Log(ex, Logger.LogLevel.Warning);
            }
            catch (OperationCanceledException ex)
            {
                new MessageBoxLogger().Log("Create failed", Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        
        private async Task UpdateObject()
        {
            try
            {
                if (CanUpdateObject())
                {
                    _searchViewModel.UpdateSelectedCompany(_dataViewModel, _tradersViewModel);
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ICompanyObjectService>();
                    await service.UpdateAsync(_searchViewModel.CompanyObjectsSearchBox.Value.Value);
                    _dataViewModel.InvokePropertyChange(nameof(_dataViewModel.Town));
                }
                else throw new ArgumentException("Not Updated");
            }
            catch (ArgumentException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning);
            }
            catch (OperationCanceledException ex)
            {
                new MessageBoxLogger().Log("Update failed", Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
            catch (DbUpdateException ex)
            {
                new MessageBoxLogger().Log("Update failed", Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }

        
        private async Task DeleteObject()
        {
            try
            {
                if (CanDeleteObject())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ICompanyObjectService>();
                    await service.DeleteSoftAsync(_searchViewModel.CompanyObjectsSearchBox.Value.Value);
                    _searchViewModel.CompanyObjectsSearchBox.Remove(_searchViewModel.CompanyObjectsSearchBox.Value.Value);
                }
                else throw new ArgumentException("Not Deleted");
            }
            catch (ArgumentException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning);
            }
            catch (OperationCanceledException ex)
            {
                new MessageBoxLogger().Log("Delete failed", Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
            catch (DbUpdateException ex)
            {
                new MessageBoxLogger().Log("Delete failed", Logger.LogLevel.Error).Log(ex, Logger.LogLevel.Error);
            }
        }
        private bool CanCreateObject()
        {
            if (_companiesSearchView.CompaniesSearchBox.Value.Value != null
                && _tradersViewModel.TraderViewModel.Value.Value != null
                && _searchViewModel.CompanyObjectsSearchBox.TextValue != null
                && _searchViewModel.CompanyObjectsSearchBox.TextValue.Length > 2
                && _searchViewModel.CompanyObjectsSearchBox.Value.Value == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool CanUpdateObject()
        {
            if (_searchViewModel.CompanyObjectsSearchBox.Value.Value != null
                && (_searchViewModel.CompanyObjectsSearchBox.Value.Value.Address == null
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.Address?.Town != _dataViewModel.Town
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.Address?.StreetName != _dataViewModel.Street
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.Address?.Number != _dataViewModel.Number
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.Address?.GpsCoordinates != _dataViewModel.GpsCoordinates
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.Address?.Description != _dataViewModel.Description
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.IsBankPay != _dataViewModel.BankPay
                || _searchViewModel.CompanyObjectsSearchBox.Value.Value.Trader?.Name != _tradersViewModel.TraderViewModel.Value.Value?.Name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanDeleteObject()
        {
            if (_searchViewModel.CompanyObjectsSearchBox.Value.Value != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
