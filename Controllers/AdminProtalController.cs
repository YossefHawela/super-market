using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Data;
using SuperMarket.DTO;
using SuperMarket.Mapper;
using SuperMarket.Models;
using System.Collections;
using System.Security.Claims;

namespace SuperMarket.Controllers
{

    public class AdminProtalController : Controller
    {

        private readonly DataConnector _dataConnector;

        public AdminProtalController(DataConnector dataConnector)
        {
            _dataConnector = dataConnector;
        }


        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminLogin");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            IEnumerable<UserDTO> users = _dataConnector.Users;
            return View(users);
        }

        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginAccountModel loginAccountModel) 
        {
            if (User.Identity?.IsAuthenticated == true&&User.IsInRole("Admin"))
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

                if(user.Role != "Admin")
                {

                    ModelState.AddModelError("", "This account doesn't have admin privileges");
                    return View(loginAccountModel);

                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, user.Role)

                };

                var claimsIdentity = new ClaimsIdentity(claims, "ToDoAuthenCookie");

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync("ToDoAuthenCookie", claimsPrincipal);

                return RedirectToAction("Index", "Home", null);


            }
            else
            {
                ModelState.AddModelError("", "Password is not correct!");
            }

            return View(loginAccountModel);
        }

        [Authorize(Roles = "Admin")]

        public IActionResult Settings()
        {
            // Fetch these from DB or config, don't be lazy.
            var vm = new AdminSettingsModel
            {
                SiteName = "SuperMarket",
                ContactEmail = "support@supermarket.com",
                MaintenanceMode = false
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult SaveSettings(AdminSettingsModel model)
        {
            if (!ModelState.IsValid)
                return View("Settings", model);

            // Save settings to DB or config file. You know what to do.
            TempData["Success"] = "Settings saved.";
            return RedirectToAction("Settings");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(UserDTO model)
        {
            if (!ModelState.IsValid) return View(model);

            bool isRegisterValid = true;

            if (!_dataConnector.IsEmailAvailable(model.Email))
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                isRegisterValid = false;
            }

            if (!_dataConnector.IsUserNameAvailable(model.UserName))
            {
                ModelState.AddModelError("UserName", "Username is already in use.");
                isRegisterValid = false;
            }

            if (isRegisterValid)
            {

                _dataConnector.Add(model.ToSecureUserDTO());
                return RedirectToAction("Users");
            }
            else
            {
                return View();
            }

        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUser(UserDTO model)
        {
            var user = _dataConnector.GetUserByUserName(model.UserName);
            if (user == null) return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == user.Id.ToString())
            {
                TempData["Error"] = "Self-editing denied. Try asking the Dragon Balls instead.";
                return RedirectToAction("Users");
            }

            return View(user);
        }

        [HttpPost("EditUserPost")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult EditUserPost(UserDTO model)
        {
            var user = _dataConnector.GetUserByID(model.Id);
            if (user == null) 
            {
                TempData["Error"] = $"i tired to edit {model.UserName}, but couldn't";

                return RedirectToAction("Users");
            }

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == user.Id.ToString())
            {
                TempData["Error"] = "Editing your own account? What’s next—firing yourself in the mirror?";
                return RedirectToAction("Users");
            }

            bool isRegisterValid = true;

            if (!_dataConnector.IsEmailAvailable(model.Email) && user.Email != model.Email)
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                isRegisterValid = false;
            }

            if (!_dataConnector.IsUserNameAvailable(model.UserName) && user.UserName != model.UserName)
            {
                ModelState.AddModelError("UserName", "Username is already in use.");
                isRegisterValid = false;
            }

            if (isRegisterValid)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.Role = model.Role;

                _dataConnector.Update(user);
                TempData["Success"] = $"User '{user.UserName}'  was changed. Bold of you to edit someone else's access?! You’re either fixing a problem or starting one. Can’t wait to find out.";

                return RedirectToAction("Users");
            }

            return View("EditUser", model);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(Guid id)
        {
            var user = _dataConnector.GetUserByID(id);
            if (user == null) return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == id.ToString())
            {
                TempData["Error"] = "Wow. You really tried to Thanos-snap yourself. Sit down, you’re not even purple.";
                return RedirectToAction("Users");
            }

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteUser(UserDTO model)
        {
            var user = _dataConnector.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null) return NotFound();

            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId == model.Id.ToString())
            {
                TempData["Error"] = "Trying to delete yourself? Bold move, Thanos. But no, you're not snapping yourself out of existence.";
                return RedirectToAction("Users");
            }

            _dataConnector.Delete(user);
            TempData["Success"] = $"User '{user.UserName}' has been terminated. Hasta la vista, baby.";
            return RedirectToAction("Users");
        }



    }
}
