using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands.AddNewCompanyCommands
{
    public class DefaultCommand : CommandBase
    {
        private string[] _properties;
        public DefaultCommand(Func<Task> action, Func<bool> canExecuteAction, INotifyPropertyChanged eventArg, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            eventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }

        public DefaultCommand(Func<Task> action, Func<bool> canExecuteAction, INotifyPropertyChanged eventArg, INotifyPropertyChanged secondEventArg, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            eventArg.PropertyChanged += OnViewModelChange;
            secondEventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }
        public DefaultCommand(Func<Task> action, Func<bool> canExecuteAction, INotifyPropertyChanged eventArg, INotifyPropertyChanged secondEventArg, INotifyPropertyChanged thirdEventArg, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            eventArg.PropertyChanged += OnViewModelChange;
            secondEventArg.PropertyChanged += OnViewModelChange;
            thirdEventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }

        public event Func<Task> Action;
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

