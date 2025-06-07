using System.ComponentModel.DataAnnotations;
using DelitaTrade.Common;

namespace DelitaTrade.Components.ComponentsViewModel.LoginViewModelComponents
{
    public class UserNamePlaceHolderViewModel : PlaceHolderTextBoxViewModel
    {
        public UserNamePlaceHolderViewModel(string placeHolderInitialText = "User name") : base(placeHolderInitialText)
        {            
        }

        [MinLength(ValidationConstants.UserNameMinLength, ErrorMessage = "User name must contains at least 4 symbols")]
        [MaxLength(ValidationConstants.UserNameMaxLength, ErrorMessage = "User name must contains max 100 symbols")]
        public override string TextValue 
        { 
            get => base.TextValue; 
            set => base.TextValue = value; 
        }
    }
}
