﻿using System;
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
                        return RedirectToAction("Login");
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

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(s => s.Email == email);
            if (user == null)
                return View("Error");

            if (token == CreateConfirmationToken(user))
            {
                user.EmailConfirmed = true;
                await DbContext.SaveChangesAsync();
                return View("ConfirmEmail");
            }
            else
                return View("Error");
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
        public async Task<IActionResult> EditPost()
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

            RegisterViewModel registerViewModel = _mapper.Map<RegisterViewModel>(userToUpdate);
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

        private bool SendConfirmationEmail(DbUser user)
        {
            string token = CreateConfirmationToken(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth", new { token, email = user.Email }, Request.Scheme);
            EmailProfile emailHelper = new EmailProfile();
            return emailHelper.SendEmail(user.Email, confirmationLink, "ToDoApp - Confirm your email");
        }

        private string CreateConfirmationToken(DbUser user)
        {
            string token = HashProfile.GetSaltedHashData(user.Email, user.PasswordSalt);
            return token;
        }
    }
}
