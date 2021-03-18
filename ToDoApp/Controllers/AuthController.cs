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

            SignUserCookie(user);
            return RedirectToAction("Index", "Tasks");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
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
                    SignUserCookie(userModel);
                    return RedirectToAction("Index", "Tasks");
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

        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await DbContext.Users.FindAsync(int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (user == null)
            {
                return NotFound();
            }
            LoginViewModel loginViewModel = _mapper.Map<LoginViewModel>(user);
            return View(loginViewModel);
        }

        [Authorize]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost()
        {
            var userToUpdate = await DbContext.Users.FirstOrDefaultAsync(s => s.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            if (await TryUpdateModelAsync<DbUser>(userToUpdate, "", s => s.Login, s => s.Password))
            {
                try
                {
                    userToUpdate.Password = HashProfile.GetSaltedHashPassword(userToUpdate.Password, userToUpdate.PasswordSalt);
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

            LoginViewModel loginViewModel = _mapper.Map<LoginViewModel>(userToUpdate);
            return View(loginViewModel);
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
    }
}
