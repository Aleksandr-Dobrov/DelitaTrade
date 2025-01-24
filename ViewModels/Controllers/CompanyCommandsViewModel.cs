using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Extensions;
using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DelitaTrade.ViewModels.Controllers
{
    public class CompanyCommandsViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private CompaniesSearchViewModel _companiesSearchViewModel;
        private CompanyObjectsSearchViewModel _objectsSearchViewModel;
        private ICompanyData _dataViewModel;
        public CompanyCommandsViewModel(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public void CreateCommands(CompaniesSearchViewModel searchViewModel, CompanyObjectsSearchViewModel objectsSearchViewModel, ICompanyData dataViewModel)
        {
            _companiesSearchViewModel = searchViewModel;
            _objectsSearchViewModel = objectsSearchViewModel;
            _dataViewModel = dataViewModel;
            CreateCommand = new DefaultCommand(CreateCompany, CanCreateCompany, _companiesSearchViewModel.CompaniesSearchBox, 
                                                                                _dataViewModel,
                                                                                nameof(_companiesSearchViewModel.CompaniesSearchBox.TextValue), 
                                                                                nameof(_companiesSearchViewModel.CompaniesSearchBox.Value.Value), 
                                                                                nameof(_dataViewModel.Bulstad), 
                                                                                nameof(_dataViewModel.CompanyType));

            UpdateCommand = new DefaultCommand(UpdateCompany, CanUpdateCompany, _dataViewModel, 
                                                                                nameof(_dataViewModel.Bulstad), 
                                                                                nameof(_dataViewModel.CompanyType));

            DeleteCommand = new DefaultCommand(DeleteCompany, CanDeleteCompany, _companiesSearchViewModel.CompaniesSearchBox, 
                                                                                nameof(_companiesSearchViewModel.CompaniesSearchBox.Value.Value));
        }
        private async Task CreateCompany()
        {
            try
            {
                if (CanCreateCompany())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ICompanyService>();
                    Core.ViewModels.CompanyViewModel newCompany = new()
                    {
                        Name = _companiesSearchViewModel.CompaniesSearchBox.TextValue,
                        Type = _dataViewModel.CompanyType,
                        Bulstad = _dataViewModel.Bulstad
                    };
                    newCompany.Id = await service.CreateAsync(newCompany);
                    _companiesSearchViewModel.CompaniesSearchBox.Add(newCompany);
                    _companiesSearchViewModel.CompaniesSearchBox.SelectItem(newCompany);
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

        private async Task UpdateCompany()
        {
            try
            {
                if (CanUpdateCompany())
                {
                    _companiesSearchViewModel.UpdateSelectedCompany(_dataViewModel);
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ICompanyService>();
                    await service.UpdateAsync(_companiesSearchViewModel.CompaniesSearchBox.Value.Value);
                    _dataViewModel.InvokePropertyChange(nameof(_dataViewModel.Bulstad));
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

        private async Task DeleteCompany()
        {
            try
            {
                if (CanDeleteCompany())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ICompanyService>();
                    await service.DeleteSoftAsync(_companiesSearchViewModel.CompaniesSearchBox.Value.Value);
                    _objectsSearchViewModel.CompanyObjectsSearchBox.RemoveRange(_companiesSearchViewModel.CompaniesSearchBox.Value.Value.CompanyObjects);
                    _companiesSearchViewModel.CompaniesSearchBox.Remove(_companiesSearchViewModel.CompaniesSearchBox.Value.Value);
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

        private bool CanCreateCompany()
        {
            if (_companiesSearchViewModel.CompaniesSearchBox.TextValue != null && 
                _companiesSearchViewModel.CompaniesSearchBox.HasErrors == false && 
                _companiesSearchViewModel.CompaniesSearchBox.Value.Value == null &&
                _dataViewModel.HasErrors == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanUpdateCompany()
        {
            if (_companiesSearchViewModel.CompaniesSearchBox.Value.Value != null && 
                _dataViewModel.HasErrors == false &&
                (_companiesSearchViewModel.CompaniesSearchBox.Value.Value.Bulstad != _dataViewModel.Bulstad ||
                _companiesSearchViewModel.CompaniesSearchBox.Value.Value.Type != _dataViewModel.CompanyType))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanDeleteCompany()
        {
            if (_companiesSearchViewModel.CompaniesSearchBox.Value.Value != null)
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
