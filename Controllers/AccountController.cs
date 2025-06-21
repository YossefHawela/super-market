using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using SuperMarket.Data;
using SuperMarket.Mapper;
using SuperMarket.Models;

namespace ToDoList.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataConnector _dataConnector;

        public AccountController(DataConnector dataConnector)
        {
            _dataConnector = dataConnector;
        }


        public  ActionResult Index()
        {
            return RedirectToAction("Login");
        }


        public ActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            // Return the Register view with an empty model
            return View(RegisterAccountModel.Default);
        }
       
        [HttpPost]
        public ActionResult Register(RegisterAccountModel registerAccountModel)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter valid credentials.");
                return View(registerAccountModel);
            }
            if (registerAccountModel.IsValid)
            {
                bool isRegisterValid = true;

                // Check if the Email or username is used

                if (!_dataConnector.IsEmailAvailable(registerAccountModel.Email))
                {
                    ModelState.AddModelError("Email", "Email is already in use.");
                    isRegisterValid = false;
                }

                if (!_dataConnector.IsUserNameAvailable(registerAccountModel.UserName))
                {
                    ModelState.AddModelError("UserName", "Username is already in use.");
                    isRegisterValid = false;
                }

                if (isRegisterValid)
                {
                    _dataConnector.Add(registerAccountModel.ToUserDTO(true)); // Hash the password before saving it to the database

                    return RedirectToAction("Login");

                }
                else
                {
                    // If there are validation errors, return the view with the model to display errors
                    return View(registerAccountModel);
                }


            }
            else 
            {   // If the model is not valid, return the view with the model to display validation errors
                ModelState.AddModelError("", "Email/Password or UserName are not valid!");
            }


            return View(registerAccountModel);

        }


        public ActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            // Return the Login view with an empty model
            return View(LoginAccountModel.Default);
        }
        [HttpPost]
        public async Task<ActionResult> Login(LoginAccountModel loginAccountModel)
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Please enter valid credentials.");
                return View(loginAccountModel);
            }

            if (!loginAccountModel.IsAllFieldsInputValid)
            {
                ModelState.AddModelError("", "Please enter a valid username or email.");
                return View(loginAccountModel);
            }

            var user = _dataConnector.GetUserByUserName(loginAccountModel.UserNameOrEmail);

            if (user == null)
            {
                // Try to find the user by email if not found by username
                user = _dataConnector.GetUserByEmail(loginAccountModel.UserNameOrEmail);
            }

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username/Email");
                return View(loginAccountModel);
            }

            if (loginAccountModel.ValidatePassword(user).isValid)
            {

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, "User") 

                };

                var claimsIdentity = new ClaimsIdentity(claims, "ToDoAuthenCookie");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("ToDoAuthenCookie", claimsPrincipal);

                return RedirectToAction("Index","Home",null);


            }
            else
            {
                ModelState.AddModelError("", "Password is not correct!");
            }

            return View(loginAccountModel);
        }

        [Authorize(AuthenticationSchemes = "ToDoAuthenCookie")]

        public ActionResult AccessDenied()
        {
            // Return the AccessDenied view
            return RedirectToAction("Login");
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "ToDoAuthenCookie")]
        public ActionResult LogoutView() 
        {
            return View();
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = "ToDoAuthenCookie")]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("ToDoAuthenCookie");

            return RedirectToActionPermanent("Login");
        }
    }


}
