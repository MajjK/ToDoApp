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
using Microsoft.EntityFrameworkCore;
using ToDoApp.DB.Model;
using ToDoApp.DB;
using ToDoApp.ViewModel.Auth;
using ToDoApp.ViewModel;
//If w _LoginPartial jeśli zalogowany to wyświetl Hello/LogOut jeśli nie to Create Account
// Usuwanie Cookie po wylaczeniu strony

namespace ToDoApp.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ToDoDatabaseContext DbContext;

        public AuthController(ToDoDatabaseContext context)
        {
            this.DbContext = context;
        }

        public IActionResult Login()
        {
            return this.View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Index", loginViewModel);
            }
            DbUser user = await this.DbContext.Users.Where(s => s.Login == loginViewModel.Login && s.Password == loginViewModel.Password).SingleOrDefaultAsync();
            if (user == null)
            {
                this.ModelState.AddModelError("", "Wrong login or password");
                return this.View("Index", loginViewModel);
            }

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

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
            return this.RedirectToAction("Index", "Tasks");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Login", "Auth");
        }

        public async Task<IActionResult> Create()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Login", "Auth");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
