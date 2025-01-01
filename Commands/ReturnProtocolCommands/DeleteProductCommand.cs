using DelitaTrade.Commands;
using DelitaTrade.Components.ComponentsViewModel.ReturnProtocolComponentViewModels;
using DelitaTrade.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Commands.ReturnProtocolCommands
{
    public class DeleteProductCommand : CommandBase
    {
        private ProductToReturnViewModel _row;
        public DeleteProductCommand(ProductToReturnViewModel row)
        {
            _row = row;
        }
        public override void Execute(object? parameter)
        {
            try
            {
                _row.ListViewInputViewModel.RemoveRow(_row);
            }
            catch (InvalidOperationException ex) 
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning);
            }
        }
    }
}
