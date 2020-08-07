using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataAccess.Repositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrypt;
using VendingMachineWebMVC.Models;

namespace VendingMachineWebMVC.Controllers
{
    public class UsersController : Controller
    {
        private IUserService userService = new UserService();
        private IUserRepository repo = new UserRepository();
        ClaimsIdentity identity = null;
        
        private const string secretCode = "ghujllrt";


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(UserRegisterViewModel user)
        {

            var newUser = repo.FindUser(user.UserName);

            if (newUser != null)
            {
                ModelState.AddModelError("", "This username already exists");
                return View();
            }
            else
            {
                if (user.SecretCode == secretCode)
                {
                    userService.Register(user);
                }
                else
                {
                    ModelState.AddModelError("", "This secret code is not available");
                    return View();
                }
            }

            return View();
        }
        
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(DataAccess.ViewModels.UserLoginViewModel user)
        {
            ScryptEncoder encoder = new ScryptEncoder();
            bool isAuthenticated = false;
            var founduser = repo.FindUser(user.Username);
            
            if (founduser == null)
            {
                ModelState.AddModelError("", "Ths user does not exist");
            }
            else
            {

                bool isValidAdmin = encoder.Compare(user.Password, founduser.Password);

                if (isValidAdmin)
                {
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                    isAuthenticated = true;
                    if (isAuthenticated)
                    {
                        var principal = new ClaimsPrincipal(identity);

                        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index", "Home");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Check the password");
                    }
                }
            }
            return View();
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index","Home");

        }
    }
}
