using DelitaTrade.Models.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Commands.ReturnProtocolCommands
{
    public class DeleteCommand : CommandBase
    {
        public DeleteCommand(Action action)
        {
            Action = action;
        }

        public event Action Action;
        public override void Execute(object? parameter)
        {
            try
            {
                Action.Invoke();
            }
            catch (ArgumentNullException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning);
            }
        }
    }
}
