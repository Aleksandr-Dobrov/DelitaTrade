using System.ComponentModel.DataAnnotations;
using System.Globalization;
using DelitaTrade.Common.Extensions;

namespace DelitaTrade.Common.DelitaValidations
{
    [AttributeUsage(AttributeTargets.Property,  AllowMultiple = false, Inherited = true)]
    public class IsDigitsValidationAttribute : ValidationAttribute
    {
        private int _maxLength;
        private const string _errorMessage = "Value must be only digits";
        public IsDigitsValidationAttribute(int maxLength)
        {
            _maxLength = maxLength;
            ErrorMessage = _errorMessage;
        }

        public IsDigitsValidationAttribute(int maxLength, string errorMessage)
        {
            _maxLength = maxLength;
            ErrorMessage = errorMessage;
        }
        public override bool IsValid(object? value)
        {            
            if (value is string number) return number.Length <= _maxLength && number.IsOnlyDigits();
            else if (value is int or double or decimal or float) return true;
            else return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.InvariantCulture, ErrorMessageString, name);
        }
    }
}
