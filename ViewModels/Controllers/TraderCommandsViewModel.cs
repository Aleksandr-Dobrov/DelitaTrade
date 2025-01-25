using DelitaTrade.Commands.AddNewCompanyCommands;
using DelitaTrade.Common;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Core.ViewModels;
using DelitaTrade.Extensions;
using DelitaTrade.Infrastructure.Data.Models;
using DelitaTrade.Models.Loggers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlTypes;
using System.Windows.Input;

namespace DelitaTrade.ViewModels.Controllers
{
    public class TraderCommandsViewModel
    {
        private readonly IServiceProvider _serviceProvider;
        private TradersListViewModel _tradersListViewModel;

        public TraderCommandsViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public void CreateCommands(TradersListViewModel tradersListViewModel)
        {
            _tradersListViewModel = tradersListViewModel;
            CreateCommand = new DefaultCommand(CreateTrader, CanCreateTrader,
                _tradersListViewModel.TraderViewModel, 
                nameof(_tradersListViewModel.TraderViewModel.TextValue), 
                nameof(_tradersListViewModel.TraderViewModel.Value.Value));
            UpdateCommand = new DefaultCommand(UpdateTrader, CanUpdateTrader, 
                [
                    _tradersListViewModel, 
                    _tradersListViewModel.TraderViewModel 
                ],
                nameof(_tradersListViewModel.PhoneNumber));
            DeleteCommand = new DefaultCommand(DeleteTrader, CanDeleteTrader, 
                _tradersListViewModel.TraderViewModel, 
                nameof(_tradersListViewModel.TraderViewModel.Value.Value));
        }

        private async Task CreateTrader()
        {
            try
            {
                if (CanCreateTrader())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ITraderService>();
                    TraderViewModel newTrader = new TraderViewModel
                    {
                        Name = _tradersListViewModel.TraderViewModel.TextValue,
                        PhoneNumber = _tradersListViewModel.PhoneNumber
                    };
                    newTrader.Id = await service.CreateAsync(newTrader);
                    _tradersListViewModel.TraderViewModel.Add(newTrader);
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

        private async Task UpdateTrader()
        {
            try
            {
                if (CanUpdateTrader())
                {
                    _tradersListViewModel.TraderViewModel.Value.Value.PhoneNumber = _tradersListViewModel.PhoneNumber;
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ITraderService>();
                    await service.UpdateAsync(_tradersListViewModel.TraderViewModel.Value.Value);
                    _tradersListViewModel.InvokePropertyChange(nameof(_tradersListViewModel.PhoneNumber));
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

        private async Task DeleteTrader()
        {
            try
            {
                if (CanDeleteTrader())
                {
                    using var scope = _serviceProvider.CreateScope();
                    var service = scope.GetService<ITraderService>();
                    await service.DeleteSoftAsync(_tradersListViewModel.TraderViewModel.Value.Value);
                    _tradersListViewModel.TraderViewModel.Remove(_tradersListViewModel.TraderViewModel.Value.Value);
                }
                else throw new ArgumentException("Not Deleted");
            }
            catch(ArgumentException ex)
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

        private bool CanCreateTrader()
        {
            if (_tradersListViewModel.TraderViewModel.TextValue != null
                && _tradersListViewModel.TraderViewModel.Value.Value == null
                && _tradersListViewModel.TraderViewModel.HasErrors == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanUpdateTrader()
        {
            if (_tradersListViewModel.TraderViewModel.Value.Value != null
                && _tradersListViewModel.TraderViewModel.Value.Value.PhoneNumber != _tradersListViewModel.PhoneNumber)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool CanDeleteTrader()
        {
            if (_tradersListViewModel.TraderViewModel.Value.Value != null)
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
