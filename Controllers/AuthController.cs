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
//sha512, md5 kodowanie hasła

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

        public IActionResult Index()
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

            var identity = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Login),
            }, CookieAuthenticationDefaults.AuthenticationScheme);

            var props = new AuthenticationProperties();
            var principal = new ClaimsPrincipal(identity);
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            return this.RedirectToAction("Index", "Tasks");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            DbContext.Dispose();
            base.Dispose(disposing);
        }
    }
}
