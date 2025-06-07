using DelitaTrade.Common;
using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class PasswordBoxViewModel : ValidationViewModel
    {
        private string _placeHolderInitialText;
        private string _password = string.Empty;
        private string _placeHolder = string.Empty;
        private Visibility _visibility;

        public PasswordBoxViewModel(string placeHolderInitialText)
        {
            _placeHolderInitialText = placeHolderInitialText;
            PlaceHolder = placeHolderInitialText;
        }

        public string PlaceHolder
        {
            get => _placeHolder;
            private set
            {
                _placeHolder = value;
                OnPropertyChange();
            }
        }

        public Visibility Visibility

        {
            get => _visibility;
            set 
            {
                _visibility = value;
                OnPropertyChange();
            }
        }

        [MinLength(ValidationConstants.UserPasswordMinLength, ErrorMessage = "Password must be at least 5 characters long")]
        public virtual string Password
		{
			get => _password; 
			set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    PlaceHolder = _placeHolderInitialText;
                }
                else
                {
                    PlaceHolder = string.Empty;
                }
                _password = value;
                OnPropertyChange();
            }
		}
	}
}
