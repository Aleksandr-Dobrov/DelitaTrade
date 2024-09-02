using DelitaTrade.Commands;
using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsCommands
{
    public class RemoveBanknotesCommand : CommandBase
    {
        private readonly BanknoteViewModel _banknoteViewModel;

        private readonly DelitaTradeDayReport _delitaTradeDayReport;

        public RemoveBanknotesCommand(BanknoteViewModel banknoteViewModel, DelitaTradeDayReport delitaTradeDayReport)
        {
            _banknoteViewModel = banknoteViewModel;
            _delitaTradeDayReport = delitaTradeDayReport;
        }

        public override void Execute(object? parameter)
        {
            _delitaTradeDayReport.RemoveMoney(_banknoteViewModel.Value, _banknoteViewModel.Count);
        }
    }
}
