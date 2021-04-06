using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ToDoApp.DB.Model;
using ToDoApp.DB;
using ToDoApp.ViewModel.Auth;
using ToDoApp.ViewModel;
using AutoMapper;
using ToDoApp.Services;

namespace ToDoApp.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ToDoDatabaseContext DbContext;
        private readonly IMapper _mapper;

        public AuthController(ToDoDatabaseContext context, IMapper mapper)
        {
            DbContext = context;
            _mapper = mapper;
        }

        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginViewModel);
            }
            
            DbUser user = await DbContext.Users.Where(s => s.Login == loginViewModel.Login).SingleOrDefaultAsync();
            
            if (user == null || !HashProfile.ValidatePasswords(loginViewModel.Password, user.Password, user.PasswordSalt))
            {
                ModelState.AddModelError("", "Wrong login or password");
                return View("Login", loginViewModel);
            }
            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Verify your email address");
                return View("Login", loginViewModel);
            }

            SignUserCookie(user);
            return RedirectToAction("Index", "Tasks");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Login, Password, ConfirmPassword, Email")] RegisterViewModel registerViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    registerViewModel.Password = HashProfile.GetSaltedHashData(registerViewModel.Password, registerViewModel.PasswordSalt);
                    DbUser userModel = _mapper.Map<DbUser>(registerViewModel);
                    DbContext.Add(userModel);
                    await DbContext.SaveChangesAsync();
                    if (SendConfirmationEmail(userModel))
                        return RedirectToAction("Login", "Auth");
                    else
                    {
                        ModelState.AddModelError("", "Unable to send confirmation e-mail. ");
                        DbContext.Users.Remove(userModel);
                        await DbContext.SaveChangesAsync();
                        return View(registerViewModel);
                    }
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(registerViewModel);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(EmailViewModel emailViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ForgotPassword", emailViewModel);
            }

            DbUser user = await DbContext.Users.Where(s => s.Email == emailViewModel.Email).SingleOrDefaultAsync();

            if (user == null)
            {
                ModelState.AddModelError("", "Wrong email address");
                return View("ForgotPassword", emailViewModel);
            }
            if (SendPasswordRecoveryEmail(user))
                return RedirectToAction("Login", "Auth");
            else
            {
                ModelState.AddModelError("", "Unable to send password recovery e-mail. ");
                return View("ForgotPassword", emailViewModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditPassword(string token, int id)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(s => s.UserId == id);
            if (user == null)
                return NotFound();

            if (token == CreateUserToken(user))
            {
                PasswordViewModel passwordViewModel = _mapper.Map<PasswordViewModel>(user);
                return View(passwordViewModel);
            }
            else
                return NotFound();
        }

        [HttpPost, ActionName("EditPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPasswordPost(PasswordViewModel passwordViewModel, string token, int id)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(s => s.UserId == id);
                if (await TryUpdateModelAsync<DbUser>(userToUpdate, "", s => s.Password))
                {
                    try
                    {
                        userToUpdate.Password = HashProfile.GetSaltedHashData(userToUpdate.Password, userToUpdate.PasswordSalt);
                        await DbContext.SaveChangesAsync();
                        return RedirectToAction("Login", "Auth");
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
            }

            return View(passwordViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(s => s.Email == email);
            if (user == null)
                return NotFound();

            if (token == CreateUserToken(user))
            {
                user.EmailConfirmed = true;
                await DbContext.SaveChangesAsync();
                return View("ConfirmEmail");
            }
            else
                return NotFound();
        }

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await DbContext.Users.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (user == null)
            {
                return NotFound();
            }
            RegisterViewModel registerViewModel = _mapper.Map<RegisterViewModel>(user);
            return View(registerViewModel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(s => s.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if (await TryUpdateModelAsync<DbUser>(userToUpdate, "", s => s.Login, s => s.Email, s => s.Password))
                {
                    try
                    {
                        userToUpdate.Password = HashProfile.GetSaltedHashData(userToUpdate.Password, userToUpdate.PasswordSalt);
                        await DbContext.SaveChangesAsync();
                        UpdateUserCookie(userToUpdate);
                        return RedirectToAction("Index", "Tasks");
                    }
                    catch (DbUpdateException)
                    {
                        ModelState.AddModelError("", "Unable to save changes. " +
                            "Try again, and if the problem persists, " +
                            "see your system administrator.");
                    }
                }
            }

            return View(registerViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async void UpdateUserCookie(DbUser user)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            SignUserCookie(user);
        }

        private async void SignUserCookie(DbUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
            };
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
        }

        private bool SendPasswordRecoveryEmail(DbUser user)
        {
            string token = CreateUserToken(user);
            var confirmationLink = Url.Action(nameof(EditPassword), "Auth", new { token, id = user.UserId }, Request.Scheme);
            return EmailProfile.SendEmail(user.Email, confirmationLink, "ToDoApp - Change your password");
        }

        private bool SendConfirmationEmail(DbUser user)
        {
            string token = CreateUserToken(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
            return EmailProfile.SendEmail(user.Email, confirmationLink, "ToDoApp - Confirm your email");
        }

        private string CreateUserToken(DbUser user)
        {
            string token = HashProfile.GetSaltedHashData(user.Email, user.PasswordSalt);
            return token;
        }
    }
}
