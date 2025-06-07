using System.ComponentModel.DataAnnotations;

namespace DelitaTrade.Common
{
    public class UserValidationForm
    {        
        [MaxLength(ValidationConstants.UserNameMaxLength, ErrorMessage = "User name must be no longer then 100 characters")]
        [MinLength(ValidationConstants.UserNameMinLength, ErrorMessage = "User name must be at least 4 characters long")]
        public required string LoginName { get; set; }

        [MaxLength(ValidationConstants.UserNameMaxLength, ErrorMessage = "Password must be no longer then 30 characters")]
        [MinLength(ValidationConstants.UserPasswordMinLength, ErrorMessage = "Password must be at least 5 characters long")]
        [UserPasswordValidation(ErrorMessage = "Password must contains: letters, digits and punctuation symbols")]
        public required string Password { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
