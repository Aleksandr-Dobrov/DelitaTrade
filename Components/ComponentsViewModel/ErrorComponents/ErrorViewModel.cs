using System.Collections;
using System.ComponentModel;

namespace DelitaTrade.Components.ComponentsViewModel.ErrorComponents
{
    public class ErrorViewModel : INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new();
        public bool HasErrors => _propertyErrors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IEnumerable GetErrors(string? propertyName)
        {
            return _propertyErrors.GetValueOrDefault(propertyName, null);
        }

        public void AddError(string propertyName, string errorMessage)
        {
            if (!_propertyErrors.ContainsKey(propertyName))
            {
                _propertyErrors.Add(propertyName, new List<string>());
            }
            _propertyErrors[propertyName].Add(errorMessage);
            OnErrorChange(propertyName);
        }

        public void ClearErrors(string proprtyName)
        {
            if (_propertyErrors.Remove(proprtyName))
            {
                OnErrorChange(proprtyName);
            }
        }

        private void OnErrorChange(string propertyName) 
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
