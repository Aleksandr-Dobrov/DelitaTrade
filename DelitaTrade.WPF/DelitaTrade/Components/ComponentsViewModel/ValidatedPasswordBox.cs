using DelitaTrade.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelitaTrade.Components.ComponentsViewModel
{
    public class ValidatedPasswordBox : PasswordBoxViewModel
    {
        public ValidatedPasswordBox(string placeHolderInitialText) : base(placeHolderInitialText)
        {
        }

        [MaxLength(ValidationConstants.UserNameMaxLength, ErrorMessage = "Password must be no longer then 30 characters")]
        [MinLength(ValidationConstants.UserPasswordMinLength, ErrorMessage = "Password must be at least 5 characters long")]
        [UserPasswordValidation(ErrorMessage = "Password must contains: letters, digits and punctuation symbols")]
        public override string Password
        {
            get => base.Password;
            set => base.Password = value;
        }
    }
}
