using System.Windows;
using System.Windows.Input;

namespace DelitaTrade.Commands
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        
        protected void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        protected bool Agreement(string message, string target)
        {
            MessageBoxResult result = MessageBox.Show($"{message} - {target} ?", message, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) return true;
            return false;            
        }

        public virtual bool CanExecute(object? parameter)
        {
            return true;
        }

        public abstract void Execute(object? parameter); 
    }
}
