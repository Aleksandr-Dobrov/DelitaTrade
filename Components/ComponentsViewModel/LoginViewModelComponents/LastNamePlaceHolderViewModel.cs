using DelitaTrade.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel.LoginViewModelComponents
{
    public class LastNamePlaceHolderViewModel : PlaceHolderTextBoxViewModel
    {
        public LastNamePlaceHolderViewModel(string placeHolderInitialText = "Last name") : base(placeHolderInitialText)
        {
        }

        [MinLength(ValidationConstants.DelitaUserConstants.NamesMinLength, ErrorMessage = "Last name must contain min 2 symbols")]
        [MaxLength(ValidationConstants.DelitaUserConstants.NameMaxLength, ErrorMessage = "Last name must contain max 50 symbols")]
        public override string TextValue
        {
            get => base.TextValue;
            set => base.TextValue = value;
        }
    }
}
