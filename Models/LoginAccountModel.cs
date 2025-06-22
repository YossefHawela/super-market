using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Text.RegularExpressions;

namespace SuperMarket.Models
{
    public class LoginAccountModel
    {
        public string UserNameOrEmail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool isValid { get; set; } = false; //should be set to true if the user is authenticated successfully

        public static readonly LoginAccountModel Default = new LoginAccountModel { };

        public string Role = "User"; // Default role, can be set to "Admin" or other roles as needed
        public bool IsAllFieldsInputValid
        {
            get
            {
                Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"); // Basic email validation regex
                Regex userNameRegex = new Regex(@"^[a-zA-Z0-9_]{3,}$"); // At least 3 characters, alphanumeric and underscores

                return !string.IsNullOrEmpty(UserNameOrEmail) && !string.IsNullOrEmpty(Password) && emailRegex.IsMatch(UserNameOrEmail) || userNameRegex.IsMatch(UserNameOrEmail);
            }
        }


        public override string ToString()
        {
            return $"{nameof(UserNameOrEmail)}:{UserNameOrEmail}, {nameof(Role)}:{Role}";
        }

    }
}
