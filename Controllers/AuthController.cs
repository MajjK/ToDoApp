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
using ToDoApp.ViewModel.Users;
using ToDoApp.ViewModel;
using AutoMapper;
using ToDoApp.Services;
//Uaktualnianie Cookie Po usunieciu uzytkownika
//Hasło duża litera nic nie zmienia
//Blad jesli haslo litery i cyfry

namespace ToDoApp.Controllers
{
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ToDoDatabaseContext DbContext;
        private readonly IMapper _mapper;

        public AuthController(ToDoDatabaseContext context, IMapper mapper)
        {
            this.DbContext = context;
            _mapper = mapper;
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
                return this.View("Login", loginViewModel);
            }
            
            DbUser user = await this.DbContext.Users.Where(s => s.Login == loginViewModel.Login).SingleOrDefaultAsync();
            
            if (user == null || !HashProfile.ValidatePasswords(loginViewModel.Password, user.Password, user.PasswordSalt))
            {
                this.ModelState.AddModelError("", "Wrong login or password");
                return this.View("Login", loginViewModel);
            }

            this.SignUserCookie(user);
            return this.RedirectToAction("Index", "Tasks");
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

            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authProperties);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return this.RedirectToAction("Login", "Auth");
        }

        public IActionResult Create()
        {
            return View();
        }
  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Login, Password")] UserViewModel userViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userViewModel.Password = HashProfile.GetSaltedHashPassword(userViewModel.Password, userViewModel.PasswordSalt);
                    DbUser userModel = _mapper.Map<DbUser>(userViewModel);
                    DbContext.Add(userModel);
                    await DbContext.SaveChangesAsync();
                    this.SignUserCookie(userModel);
                    return this.RedirectToAction("Index", "Tasks");
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            return View(userViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
