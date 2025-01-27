using DelitaTrade.Components.ComponentsViewModel.ErrorComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class LabeledStringTextBoxViewModel : ValidationViewModel
    {
        private string _label = "Text";
        private string _textBox = string.Empty;
        private Visibility _visibility = Visibility.Visible;
        private bool _isEnable;

        public string DefaultTextBoxValue { get; set; } = "ООД";

        public Visibility Visibility
        {
            get => _visibility; 
            set
            {
                _visibility = value;
                OnPropertyChange();
            }
        }


        public bool IsEnable
        {
            get => _isEnable;
            set
            {
                _isEnable = value;
                OnPropertyChange();
            }
        }


        public string Label 
        { 
            get => _label; 
            set
            {
                _label = value;
                OnPropertyChange();
            } 
        }
        [MinLength(1)]
        public virtual string TextBox 
        {
            get => _textBox;
            set
            {
                _textBox = value;
                OnPropertyChange();
            }
        }

        public void SetDefaultValue()
        {
            TextBox = DefaultTextBoxValue;
        }
    }
}
