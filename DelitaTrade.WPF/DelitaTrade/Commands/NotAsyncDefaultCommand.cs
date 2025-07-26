using DelitaTrade.Models.Loggers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Commands
{
    public class NotAsyncDefaultCommand : CommandBase
    {
        private string[] _properties;

        public NotAsyncDefaultCommand(Action action)
        {
            Action = action;
            CanExecuteAction += () => { return true; };
            _properties = [];
        }

        public NotAsyncDefaultCommand(Action action, Func<bool> canExecuteAction, INotifyPropertyChanged eventArg, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            eventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }
        public NotAsyncDefaultCommand(Action action, Func<bool> canExecuteAction, IEnumerable<INotifyPropertyChanged> eventArgs, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            foreach (var eventArg in eventArgs)
            {
                eventArg.PropertyChanged += OnViewModelChange;
            }
            _properties = property;
        }

        public event Action Action;

        public event Func<bool> CanExecuteAction;

        public override bool CanExecute(object? parameter)
        {
            return CanExecuteAction.Invoke();
        }
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
        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e)
        {
            foreach (var property in _properties)
            {
                if (e.PropertyName == property)
                {
                    OnCanExecuteChanged();
                }
            }
        }
    }
}
