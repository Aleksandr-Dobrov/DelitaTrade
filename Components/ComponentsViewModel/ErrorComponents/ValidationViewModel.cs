using DelitaTrade.ViewModels;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace DelitaTrade.Components.ComponentsViewModel.ErrorComponents
{
    public class ValidationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly Dictionary<string, List<string>> _propertyErrors = new();

        public ValidationViewModel()
        {
            PropertyChanged += OnViewModelChange;
        }

        private void OnViewModelChange(object? sender, PropertyChangedEventArgs e)
        {
            if (sender == null) return;
            Validate(sender, e.PropertyName);
        }

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

        public void ClearErrors(string propertyName)
        {
            if (_propertyErrors.Remove(propertyName))
            {
                OnErrorChange(propertyName);
            }
        }

        public void Validate(bool validationArgument,[CallerMemberName] string property = null)
        {
            ClearErrors(property);
            if (validationArgument)
            {
                AddError(property, string.Empty);
            }
        }
        /// <summary>
        /// Validation for property by ValidationAttributes
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property">Property name to validate. If parameter is null, name will take automatically</param>
        public void Validate(object obj, [CallerMemberName] string? property = null)
        {           
            var prop = obj.GetType().GetProperties().FirstOrDefault(p => p.Name == property);
            if (prop == null || property == null) return;
            var attribute = prop.GetCustomAttributes<ValidationAttribute>();
                ClearErrors(property);
            foreach (var attr in attribute)
            {
                if (!attr.IsValid(prop.GetValue(obj)))
                {
                    AddError(property, string.Empty);
                }
            }
        }

        private void OnErrorChange(string propertyName) 
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}
