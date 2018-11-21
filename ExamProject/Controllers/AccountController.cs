using ExamProject.Models;
using ExamProject.Models.Entities;
using ExamProject.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExamProject.Controllers
{
    /// <summary>
    /// Controller for account actions
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext context;

        public AccountController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var model = new AccountPageViewModel
            {
                LoginViewModel = new LoginViewModel(),
                RegisterViewModel = new RegisterViewModel()
            };

            return View(model);
        }

        public IActionResult Register(RegisterViewModel model)
        {
            var accountPageModel = new AccountPageViewModel
            {
                LoginViewModel = new LoginViewModel(),
                RegisterViewModel = model
            };

            if (model == null)
            {
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View("Index", accountPageModel);
            }

            // Check to see if email is unique
            using (context)
            {
                var dbUser = context.Users.FirstOrDefault(u => u.Email.ToUpper() == model.Email.ToUpper());
                if (dbUser != null)
                {
                    // user exists
                    ModelState.AddModelError("", "There was an error with your information. Please check and try again");
                    return View("Index", accountPageModel);
                }

                // Create the user
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                var user = new User
                {
                    Alias = model.Alias,
                    Email = model.Email,
                    Name = model.Name
                };

                var hashedPassword = hasher.HashPassword(user, model.Password);
                user.Password = hashedPassword;
                context.Add(user);
                context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var accountViewModel = new AccountPageViewModel
            {
                LoginViewModel = model,
                RegisterViewModel = new RegisterViewModel()
            };

            if (model == null)
            {
                return View("Index", accountViewModel);
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "There was a problem with your username or password");
                return View("Index", accountViewModel);
            }

            // Check the database for a user
            using (context)
            {
                var user = context.Users.FirstOrDefault(u => u.Email.ToUpper() == model.Email.ToUpper());
                if (user == null)
                {
                    ModelState.AddModelError("", "There was a problem with your username or password");
                    return View("Index", accountViewModel);
                }

                // Check the verify the hashed password
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
                var hashedPassword = passwordHasher.HashPassword(user, model.Password);

                var result = passwordHasher.VerifyHashedPassword(user, hashedPassword, model.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    // User is authenticated
                    // Create claims
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("FullName", user.Name),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    // Create claims identity
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "There was a problem with your username or password");
            return View("Index", accountViewModel);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}