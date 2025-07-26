using DelitaTrade.Models.Loggers;
using DelitaTrade.ViewModels;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace DelitaTrade.Commands
{
    public class DefaultCommand : CommandBase
    {
        private string[] _properties;

        public DefaultCommand(Func<Task> action) 
        {
            Action = action;
            CanExecuteAction += () => { return true; };
            _properties = [];
        }
        public DefaultCommand(Func<Task> action, Func<bool> canExecuteAction, INotifyPropertyChanged eventArg, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            eventArg.PropertyChanged += OnViewModelChange;
            _properties = property;
        }

        public DefaultCommand(Func<Task> action, Func<bool> canExecuteAction, IEnumerable<INotifyPropertyChanged> eventArgs, params string[] property)
        {
            Action = action;
            CanExecuteAction = canExecuteAction;
            foreach (var eventArg in eventArgs) 
            {
                eventArg.PropertyChanged += OnViewModelChange;
            }
            _properties = property;
        }

        public event Func<Task> Action;
        public event Func<bool> CanExecuteAction;

        public override bool CanExecute(object? parameter)
        {
            return CanExecuteAction.Invoke();
        }
        public override async void Execute(object? parameter)
        {
            await OnExecute();
        }
        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e)
        {
            foreach (var property in _properties)
            {
                if (e.PropertyName == property)
                {
                    Application.Current.Dispatcher.Invoke(new Action(OnCanExecuteChanged));
                }
            }
        }

        private async Task OnExecute()
        {
            try
            {
                await Action.Invoke();
            }
            catch (AggregateException ex)
            {
                new MessageBoxLogger().Log(ex, Logger.LogLevel.Warning);
            }
        }
        
    }
}

