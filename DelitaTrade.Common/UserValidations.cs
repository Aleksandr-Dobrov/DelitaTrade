namespace DelitaTrade.Common
{
    public class UserValidations
    {
        public static bool UserNameMinLengthValidation(string userName)
        {
            if(userName == null || userName.Length < ValidationConstants.UserNameMinLength) return false;
            return true;
        }

        public static bool UserNameMaxLengthValidation(string username)
        {
            return username.Length <= ValidationConstants.UserNameMaxLength;
        }

        public static bool UserPasswordMinLengthValidation(string password)
        {
            if (string.IsNullOrEmpty(password) || (password.Length < ValidationConstants.UserPasswordMinLength)) return false;
            return true;
        }

        public static bool UserPasswordMaxLengthValidation(string password)
        {
            return password.Length <= ValidationConstants.UserPasswordMaxLength;
        }

        public static bool UserPasswordValidation(string password)
        {
            bool isValidPunctuacion = false;
            bool isValidDigit = false;
            bool isValidLetter = false;
            foreach (var ch in password)
            {
                if (char.IsPunctuation(ch)) isValidPunctuacion = true;
                if (char.IsDigit(ch)) isValidDigit = true;
                if (char.IsLetter(ch)) isValidLetter = true;
            }
            return isValidPunctuacion && isValidDigit && isValidLetter;
        }
    }
}
