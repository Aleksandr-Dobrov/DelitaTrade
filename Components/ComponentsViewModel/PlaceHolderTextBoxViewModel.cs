using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using DelitaTrade.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class PlaceHolderTextBoxViewModel : ValidationViewModel
    {
		private string _placeHolderInitialText;
		private string _placeHolder = string.Empty;
		private string _textValue = string.Empty;
        private bool _isEditable = true;

        public PlaceHolderTextBoxViewModel(string placeHolderInitialText)
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

		private Visibility _visibility;

		public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChange();
            }
        }

        public bool IsEditable
        {
            get => _isEditable;
            set
            {
                _isEditable = value;
                OnPropertyChange();
            }
        }

        public virtual string TextValue
		{
			get => _textValue;
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
					_textValue = value; 
				OnPropertyChange();
			}
		}

	}
}
