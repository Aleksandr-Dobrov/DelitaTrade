﻿using DelitaTrade.Commands;
using DelitaTrade.Components.ComponetsViewModel;
using DelitaTrade.Models;
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
