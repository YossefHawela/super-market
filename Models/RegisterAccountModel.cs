using System.Text.RegularExpressions;

namespace SuperMarket.Models
{
    public class RegisterAccountModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Role = "User"; // Default role, can be set to "Admin" or other roles as needed
        public bool IsValid
        {
            get
            {
                Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"); // Basic email validation regex
                Regex passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"); // At least 8 characters, one uppercase, one lowercase, one number, and one special character
                Regex userNameRegex = new Regex(@"^[a-zA-Z0-9_]{3,}$"); // At least 3 characters, alphanumeric and underscores
         
                return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password) && emailRegex.IsMatch(Email) && passwordRegex.IsMatch(Password) &&
                       Password == ConfirmPassword && userNameRegex.IsMatch(UserName);
            }
        }

        public static readonly RegisterAccountModel Default = new RegisterAccountModel {};

    }
}
