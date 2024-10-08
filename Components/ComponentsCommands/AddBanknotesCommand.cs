using DelitaTrade.Commands;
using DelitaTrade.Components.ComponentsViewModel;
using DelitaTrade.Models;

namespace DelitaTrade.Components.ComponentsCommands
{
    public class AddBanknotesCommand : CommandBase
    {
        private readonly BanknoteViewModel _banknoteViewModel;

        private readonly DelitaTradeDayReport _delitaTradeDayReport;

        public AddBanknotesCommand(BanknoteViewModel banknoteViewModel, DelitaTradeDayReport delitaTradeDayReport)
        {
            _banknoteViewModel = banknoteViewModel;
            _delitaTradeDayReport = delitaTradeDayReport;
        }

        public override void Execute(object? parameter)
        {
            _delitaTradeDayReport.AddMoney(_banknoteViewModel.Value, _banknoteViewModel.Count);
        }
    }
}
