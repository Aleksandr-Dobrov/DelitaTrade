using DelitaTrade.Common;
using DelitaTrade.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class SearchComboBoxDescriptionValidationViewModel<T> : SearchComboBoxViewModel<T> where T : INamed
    {
        [MinLength(0)]
        [MaxLength(ValidationConstants.ReturnedProductDescriptionMaxLength, ErrorMessage = $"Description must be no longer then 100 characters")]
        public override string TextValue
        {
            get => _textValue;
            set
            {
                if (!Value.IsValueCleared) 
                {
                    _textValue = value;
                    OnPropertyChange();
                }
            }
        }
    }
}
