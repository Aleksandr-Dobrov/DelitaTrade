using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using System.ComponentModel;

namespace DelitaTrade.Commands.AddNewCompanyCommands
{
    public class DefaultCommand : CommandBase
    {
        private string[] _properties;
        public DefaultCommand(Func<Task> action, Func<bool> canAction, ViewModelBase eventArg, params string[] property)
        {
            Action = action;
            CanAction = canAction;
            eventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }

        public DefaultCommand(Func<Task> action, Func<bool> canAction, ViewModelBase eventArg, ViewModelBase secondEventArg, params string[] property)
        {
            Action = action;
            CanAction = canAction;
            eventArg.PropertyChanged += OnViewModelChange;
            secondEventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }
        public DefaultCommand(Func<Task> action, Func<bool> canAction, ViewModelBase eventArg, ViewModelBase secondEventArg, ViewModelBase thirdEventArg, params string[] property)
        {
            Action = action;
            CanAction = canAction;
            eventArg.PropertyChanged += OnViewModelChange;
            secondEventArg.PropertyChanged += OnViewModelChange;
            thirdEventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }

        public event Func<Task> Action;
        public event Func<bool> CanAction;

        public override bool CanExecute(object? parameter)
        {
            return CanAction.Invoke();
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

