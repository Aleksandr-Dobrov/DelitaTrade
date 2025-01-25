using System;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Common
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UserPasswordValidationAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "Password must contain letters, digits and punctuation";
        public UserPasswordValidationAttribute()
        {
            ErrorMessage = _defaultErrorMessage;
        }
        public override bool IsValid(object? value)
        {
            if (value is string password)
            {
                bool isValidPunctuation = false;
                bool isValidDigit = false;
                bool isValidLetter = false;
                foreach (var ch in password)
                {
                    if (char.IsPunctuation(ch)) isValidPunctuation = true;
                    if (char.IsDigit(ch)) isValidDigit = true;
                    if (char.IsLetter(ch)) isValidLetter = true;
                }
                return isValidPunctuation && isValidDigit && isValidLetter;
            }
            return false;
        }

        public override string FormatErrorMessage(string name)
        {            
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }
    }
}
