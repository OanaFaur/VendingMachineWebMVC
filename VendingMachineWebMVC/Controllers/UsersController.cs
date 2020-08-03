using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess.Repositories;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Scrypt;
using VendingMachineWebMVC.DataContext;
using VendingMachineWebMVC.Models;

using VendingMachineWebMVC.ViewModel;

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

            // var password = founduser.Password;

            //if (founduser != null)
            //{

            //    if (userService.Login(user))
            //    {
            //        return View("LoginSuccesful");
            //    }
            //    return View();
            // }

            if (founduser == null)
            {
                ModelState.AddModelError("", "Ths user does not exist");
            }
            //else if( founduser!=null)
            // {

            //     if (userService.Login(user))
            //     {
            //         return View("LoginSuccesful");
            //     }
            //     else
            //     {
            //         ModelState.AddModelError("", "Check the password");


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

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // Create hash using password salt.
                for (int i = 0; i < computedHash.Length; i++)
                { // Loop through the byte array
                    if (computedHash[i] != passwordHash[i]) return false; // if mismatch
                }
            }
            return true; //if no mismatches.
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
