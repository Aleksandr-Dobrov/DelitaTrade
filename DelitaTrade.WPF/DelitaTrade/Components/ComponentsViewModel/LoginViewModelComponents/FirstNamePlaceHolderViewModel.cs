using DelitaTrade.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel.LoginViewModelComponents
{
    public class FirstNamePlaceHolderViewModel : PlaceHolderTextBoxViewModel
    {
        public FirstNamePlaceHolderViewModel(string placeHolderInitialText = "First name") : base(placeHolderInitialText)
        {
        }

        [Required(ErrorMessage = "First name is required")]
        [MinLength(ValidationConstants.DelitaUserConstants.NamesMinLength, ErrorMessage = "First name must contain min 2 symbols")]
        [MaxLength(ValidationConstants.DelitaUserConstants.NameMaxLength, ErrorMessage = "First name must contain max 50 symbols")]
        public override string TextValue
        {
            get => base.TextValue;
            set => base.TextValue = value;
        }
    }
}
