using DelitaTrade.Commands;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Core.Contracts;
using DelitaTrade.Models;
using DelitaTrade.Models.Loggers;
using Microsoft.Extensions.DependencyInjection;

namespace DelitaTrade.Components.ComponentsCommands
{
    public class AddBanknotesCommand : CommandBase
    {
        private readonly BanknoteViewModel _banknoteViewModel;

        private readonly IServiceProvider _serviceProvider;

        public AddBanknotesCommand(BanknoteViewModel banknoteViewModel, IServiceProvider serviceProvider)
        {
            _banknoteViewModel = banknoteViewModel;
            _serviceProvider = serviceProvider;
        }

        public override async void Execute(object? parameter)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var service = scope.GetService<IBanknotesService>();
                await service.AddMoneyAsync(_banknoteViewModel.DayReportId, _banknoteViewModel.Value, _banknoteViewModel.Count);
                _banknoteViewModel.OnBanknoteChange();
            }        
            catch (ArgumentException ex)
            {
                new MessageBoxLogger().Log(ex.Message, Logger.LogLevel.Information);
            }
        }
    }
}
